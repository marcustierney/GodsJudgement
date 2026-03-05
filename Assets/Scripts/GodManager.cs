using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GodManager : MonoBehaviour
{
    public static GodManager Instance;
    public float thinkInterval = 10f;
    float timer;
    public List<GodAction> actions = new List<GodAction>();
    private GodContext context;
    private int troopDeaths = 0;
    private int enemyDeaths = 0;
    private int troopKills = 0;
    private float prevMoralitySatisfaction = 0.5f;
    private float prevStyleSatisfaction = 0.5f;
    private float prevConsumptionSatisfaction = 0.5f;
    private List<GodHint> activeHints = new List<GodHint>();
    private float hintDuration = 6f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        context = new GodContext();
        context.personality = new GodPersonality();
        //Randomize personality
        context.personality.morality = (MoralityType)Random.Range(0, 2);
        context.personality.style = (StyleType)Random.Range(0, 2);
        context.personality.consumption = (ConsumptionType)Random.Range(0, 2);
        //Give all satisfactions a neutral starting value
        context.personality.moralitySatisfaction = 0.5f;
        context.personality.styleSatisfaction = 0.5f;
        context.personality.consumptionSatisfaction = 0.5f;
        Debug.Log("Morality: " + context.personality.morality);
        Debug.Log("Style: " + context.personality.style);
        Debug.Log("Consumption: " + context.personality.consumption);
        actions.Add(new SpawnEnemiesAction());
        actions.Add(new BuffTroopsAction());
        actions.Add(new FamineAction());
        actions.Add(new EarthquakeAction());
        actions.Add(new DesertTroopsAction());
        actions.Add(new HealTroopsAction());
        actions.Add(new ArmorTroopsAction());
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= thinkInterval)
        {
            timer = 0;
            UpdateContext();
            EvaluateSatisfaction();
            CheckHints();
            CheckWinCondition();
            ChooseAction();
        }
        for (int i = activeHints.Count - 1; i >= 0; i--)
        {
            activeHints[i].timeRemaining -= Time.deltaTime;
            if (activeHints[i].timeRemaining <= 0)
            {
                activeHints.RemoveAt(i);
            }
        }
    }

    private class GodHint
    {
        public string message;
        public bool pleased;
        public float timeRemaining;
    }

    void UpdateContext()
    {
        context.troopCount = GameObject.FindGameObjectsWithTag("Friendly").Length;
        context.enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        context.food = ResourceManager.Instance.food;
        context.wood = ResourceManager.Instance.wood;
        context.troopDeaths = troopDeaths;
        context.enemyDeaths = enemyDeaths;
        context.troopKills = troopKills;
        context.buildingSpread = CalculateBuildingSpread();
    }

    void EvaluateSatisfaction()
    {
        GodPersonality p = context.personality;
        if (p.morality == MoralityType.Peaceful) //Satisfied when few deaths on either side prefers preservation
        {
            float deathPenalty = Mathf.Clamp01((troopDeaths + enemyDeaths) / 20f);
            p.moralitySatisfaction = 1f - deathPenalty;
        }
        else //Satisfied when lots of kills and troop deaths (combat happening)
        {
            p.moralitySatisfaction = Mathf.Clamp01((troopKills + troopDeaths) / 20f);
        }

        if (p.style == StyleType.Wild) //Satisfied when buildings are spread apart
        {
            p.styleSatisfaction = Mathf.Clamp01(context.buildingSpread / 20f);
        }
        else //Satisfied when buildings are close together
        {
            p.styleSatisfaction = Mathf.Clamp01(1f - (context.buildingSpread / 20f));
        }

        if (p.consumption == ConsumptionType.Glutton) //Satisfied when resources are low
        {
            float resourceLevel = Mathf.Clamp01((context.food + context.wood) / 200f);
            p.consumptionSatisfaction = 1f - resourceLevel;
        }
        else //Satisfied when resources are high
        {
            p.consumptionSatisfaction = Mathf.Clamp01((context.food + context.wood) / 200f);
        }
        Debug.Log($"Satisfaction - Morality: {p.moralitySatisfaction:F2} " + $"Style: {p.styleSatisfaction:F2} " + $"Consumption: {p.consumptionSatisfaction:F2} " + $"Overall: {p.OverallSatisfaction:F2}");
    }

    float CalculateBuildingSpread()
    {
        GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
        if (buildings.Length < 2)
        {
            return 0f;
        }
        float totalDist = 0f;
        int pairs = 0;
        for (int i = 0; i < buildings.Length; i++)
        {
            for (int j = i + 1; j < buildings.Length; j++)
            {
                totalDist += Vector3.Distance(buildings[i].transform.position, buildings[j].transform.position);
                pairs++;
            }
        }
        if (pairs > 0)
        {
            return totalDist / pairs;
        }
        else
        {
            return 0f;
        }
    }

    void ChooseAction()
    {
        float bestScore = 0;
        GodAction best = null;
        foreach (var action in actions)
        {
            float score = action.CalculateUtility(context);
            Debug.Log(action.GetType().Name + " score: " + score);
            if (score > bestScore)
            {
                bestScore = score;
                best = action;
            }
        }
        best.Execute(context);
    }


    void CheckWinCondition()
    {
        GodPersonality p = context.personality;
        float threshold = 0.80f;
        if (p.moralitySatisfaction >= threshold && p.styleSatisfaction >= threshold && p.consumptionSatisfaction >= threshold)
        {
            SceneManager.LoadScene("WinScene");
        }
    }

    void CheckHints()
    {
        GodPersonality p = context.personality;
        float threshold = 0.10f;
        CheckSatisfactionChange(prevMoralitySatisfaction, p.moralitySatisfaction, "morals", threshold);
        CheckSatisfactionChange(prevStyleSatisfaction, p.styleSatisfaction, "style", threshold);
        CheckSatisfactionChange(prevConsumptionSatisfaction, p.consumptionSatisfaction, "consumption", threshold);
        //Save current values for next tick comparison
        prevMoralitySatisfaction = p.moralitySatisfaction;
        prevStyleSatisfaction = p.styleSatisfaction;
        prevConsumptionSatisfaction = p.consumptionSatisfaction;
    }

    void CheckSatisfactionChange(float prev, float current, string label, float threshold)
    {
        float delta = current - prev;
        if (delta >= threshold)
        {
            AddHint($"The God is pleased with your {label}.", pleased: true);
        }
        else if (delta <= -threshold)
        {
            AddHint($"The God is displeased with your {label}.", pleased: false);
        }
    }
    void AddHint(string message, bool pleased)
    {
        activeHints.Add(new GodHint {message = message, pleased = pleased, timeRemaining = hintDuration});
        Debug.Log(message);
    }

    void OnGUI() //UI
    {
        if (activeHints.Count == 0)
        {
            return;
        }

        float startY = Screen.height / 2f + 150;
        float padding = 8f;
        float boxHeight = 36f;

        for (int i = 0; i < activeHints.Count; i++)
        {
            GodHint hint = activeHints[i];
            float alpha = Mathf.Clamp01(hint.timeRemaining / hintDuration);
            Color bgColor;
            if (hint.pleased)
            {
                bgColor = new Color(0f, 0.5f, 0f, 0.7f * alpha); //Green
            }
            else
            {
                bgColor = new Color(0.5f, 0f, 0f, 0.7f * alpha); //Red
            }
            GUI.color = bgColor;
            float y = startY + i * (boxHeight + padding);
            Rect boxRect = new Rect(Screen.width / 2f - 200f, y, 400f, boxHeight);
            GUI.DrawTexture(boxRect, Texture2D.whiteTexture);
            GUIStyle style = new GUIStyle();
            style.fontSize = 18;
            style.alignment = TextAnchor.MiddleCenter;
            style.normal.textColor = new Color(1f, 1f, 1f, alpha);
            GUI.color = Color.white;
            GUI.Label(boxRect, hint.message, style);
        }
    }

    public void RegisterTroopDeath()
    {
        troopDeaths++;
    }
    public void RegisterEnemyDeath() 
    {
        enemyDeaths++;
    }
    public void RegisterTroopKill()
    {
        troopKills++;
    }
}