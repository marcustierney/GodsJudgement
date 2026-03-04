using UnityEngine;

public class FamineAction : GodAction
{
    public override float CalculateUtility(GodContext context)
    {
        if (context.food > 200)
        {
            return 0.5f * context.greed;
        }
        return 0;
    }

    public override void Execute(GodContext context)
    {
        ResourceManager.Instance.food *= 0.5f;
        Debug.Log("God caused famine");
    }
}