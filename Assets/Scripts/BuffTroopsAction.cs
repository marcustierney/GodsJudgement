using UnityEngine;

public class BuffTroopsAction : GodAction
{
    public override float CalculateUtility(GodContext context)
    {
        float baseValue = 0;
        if (context.troopCount < 5)
        {
            baseValue = 0.4f;
        }
        return baseValue * (1 - context.aggression);
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
        }

        Debug.Log("God buffed troops");
    }
}