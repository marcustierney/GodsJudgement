using UnityEngine;

public class ArmorTroopsAction : GodAction
{
    public override float CalculateUtility(GodContext context)
    {
        if (context.personality.morality != MoralityType.Peaceful) return 0;
        if (context.personality.moralitySatisfaction < 0.7f) return 0;

        return context.personality.moralitySatisfaction * 0.7f;
    }

    public override void Execute(GodContext context)
    {
        GameObject[] troops = GameObject.FindGameObjectsWithTag("Friendly");
        foreach (var troop in troops)
        {
            TroopStats stats = troop.GetComponent<TroopStats>();
            if (stats != null)
                stats.ApplyDamageReduction(0.2f); //20% damage reduction
        }
        GodActionPopup.Instance.ShowPopup($"The God granted troops armor.");
    }
}