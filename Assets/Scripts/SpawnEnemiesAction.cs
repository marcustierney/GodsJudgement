using UnityEngine;

public class SpawnEnemiesAction : GodAction
{
    public override float CalculateUtility(GodContext context)
    {
        float dissatisfaction = 1f - context.personality.OverallSatisfaction; //scales with dissatisfaction
        if (dissatisfaction < 0.6f) return 0;
        float violentBonus = context.personality.morality == MoralityType.Violent ? 0.3f : 0f;
        return dissatisfaction * 0.8f + violentBonus;
    }

    public override void Execute(GodContext context)
    {
        EnemySpawner spawner = GameObject.FindObjectOfType<EnemySpawner>();
        if (spawner == null)
        {
            return;
        }
        //Spawn more enemies the more dissatisfied the god is
        float dissatisfaction = 1f - context.personality.OverallSatisfaction;
        int amount = Mathf.RoundToInt(Mathf.Lerp(3, 12, dissatisfaction));
        spawner.SpawnEnemies(amount);
        Debug.Log($"God spawned {amount} enemies (dissatisfaction: {dissatisfaction:F2})");
    }
}