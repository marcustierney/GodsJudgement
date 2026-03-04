using UnityEngine;

public class FamineAction : GodAction
{
    public override float CalculateUtility(GodContext context)
    {
        if (context.personality.consumption != ConsumptionType.Glutton)
        {
            return 0;
        }
        //Punish when dissatisfied (player has too many resources)
        if (context.personality.consumptionSatisfaction > 0.4f)
        {
            return 0;
        }
        return (1f - context.personality.consumptionSatisfaction) * 0.9f;
    }

    public override void Execute(GodContext context)
    {
        ResourceManager.Instance.food *= 0.5f;
        Debug.Log("Glutton caused famine punishing resource hoarding");
    }
}