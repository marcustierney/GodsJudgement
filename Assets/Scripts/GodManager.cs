using System.Collections.Generic;
using UnityEngine;

public class GodManager : MonoBehaviour
{
    public static GodManager Instance;

    public float thinkInterval = 10f;
    float timer;

    public List<GodAction> actions = new List<GodAction>();

    private GodContext context;

    private int troopDeaths = 0;
    private int enemyDeaths = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        context = new GodContext();

        context.aggression = Random.Range(0f, 1f);
        context.greed = Random.Range(0f, 1f);
        context.chaos = Random.Range(0f, 1f);
        Debug.Log("God Personality:");
        Debug.Log("Aggression: " + context.aggression);
        Debug.Log("Greed: " + context.greed);
        Debug.Log("Chaos: " + context.chaos);

        actions.Add(new SpawnEnemiesAction());
        actions.Add(new BuffTroopsAction());
        actions.Add(new FamineAction());
        actions.Add(new EarthquakeAction());
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= thinkInterval)
        {
            timer = 0;
            UpdateContext();
            ChooseAction();
        }
    }

    void UpdateContext()
    {
        context.troopCount = GameObject.FindGameObjectsWithTag("Friendly").Length;
        context.enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        context.food = ResourceManager.Instance.food;
        context.wood = ResourceManager.Instance.wood;

        context.troopDeaths = troopDeaths;
        context.enemyDeaths = enemyDeaths;
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
        if (best != null)
        {
            Debug.Log("God chose: " + best.GetType().Name);
            best.Execute(context);
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
}