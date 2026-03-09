using UnityEngine;

public class SpawnEnemiesAction : GodAction
{
    public override float CalculateUtility(GodContext context)
    {
        float dissatisfaction = 1f - context.personality.OverallSatisfaction; //scales with dissatisfaction
        if (dissatisfaction < 0.6f) return 0;
        float violentBonus;
        if (context.personality.morality == MoralityType.Violent)
        {
            violentBonus = 0.3f;
        }
        else
        {
            violentBonus = 0f;
        }
        return dissatisfaction * 0.8f + violentBonus;
    }

    public override void Execute(GodContext context)
    {
        float dissatisfaction = 1f - context.personality.OverallSatisfaction; //Spawn more enemies the more dissatisfied the god is
        int amount = Mathf.RoundToInt(Mathf.Lerp(3, 12, dissatisfaction));
        if (WaveSpawner.Instance != null) 
        {
            WaveSpawner.Instance.SpawnBonus(amount);
        }
        else
        {
            EnemySpawner spawner = GameObject.FindObjectOfType<EnemySpawner>();
            spawner.SpawnEnemies(amount);
        }
        GodActionPopup.Instance.ShowPopup("The God sent more enemies.");
        Debug.Log($"God spawned {amount} enemies (dissatisfaction: {dissatisfaction:F2})");
    }
}