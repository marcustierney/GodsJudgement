using UnityEngine;

public class HealTroopsAction : GodAction
{
    public override float CalculateUtility(GodContext context)
    {
        if (context.personality.morality != MoralityType.Peaceful) return 0;

        //Only reward when satisfied
        if (context.personality.moralitySatisfaction < 0.6f) return 0;

        return context.personality.moralitySatisfaction * 0.8f;
    }

    public override void Execute(GodContext context)
    {
        GameObject[] troops = GameObject.FindGameObjectsWithTag("Friendly");
        foreach (var troop in troops)
        {
            Health h = troop.GetComponent<Health>();
            if (h != null && !h.isDead)
            {
                h.Heal(20f);
            }
        }
        GodActionPopup.Instance.ShowPopup($"The God healed your troops.");
    }
}