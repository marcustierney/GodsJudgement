using UnityEngine;

public class BuffTroopsAction : GodAction
{
    public override float CalculateUtility(GodContext context)
    {
        if (context.personality.morality != MoralityType.Violent)
        {
            return 0;
        }
        //Only reward when satisfied with combat
        if (context.personality.moralitySatisfaction < 0.6f)
        {
            return 0;
        }
        return context.personality.moralitySatisfaction * 0.75f;
    }

    public override void Execute(GodContext context)
    {
        GameObject[] troops = GameObject.FindGameObjectsWithTag("Friendly");
        foreach (var troop in troops)
        {
            UnitCombat combat = troop.GetComponent<UnitCombat>();
            if (combat != null)
            {
                combat.damage *= 1.2f;
            }
            UnitMovement movement = troop.GetComponent<UnitMovement>();
            if (movement != null)
            {
                movement.speed *= 1.1f;
            }
        }
        Debug.Log("Violent God rewarded troops with combat buffs");
    }
}