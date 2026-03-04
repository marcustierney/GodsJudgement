using UnityEngine;

public class DesertTroopsAction : GodAction
{
    public override float CalculateUtility(GodContext context)
    {
        if (context.personality.morality != MoralityType.Peaceful) return 0;

        // Only punish when dissatisfied
        if (context.personality.moralitySatisfaction > 0.4f) return 0;

        return (1f - context.personality.moralitySatisfaction) * 0.9f;
    }

    public override void Execute(GodContext context)
    {
        GameObject[] troops = GameObject.FindGameObjectsWithTag("Friendly");
        if (troops.Length == 0) return;

        // Remove 1-2 random troops
        int desertCount = Mathf.Min(Random.Range(1, 3), troops.Length);
        for (int i = 0; i < desertCount; i++)
        {
            GameObject troop = troops[Random.Range(0, troops.Length)];
            if (troop != null)
            {
                Object.Destroy(troop);
                ResourceManager.Instance.DeregisterTroop();
            }
        }
        Debug.Log($"Peaceful God dissatisfied — {desertCount} troop(s) deserted.");
    }
}