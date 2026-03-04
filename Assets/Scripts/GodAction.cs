using UnityEngine;

public abstract class GodAction
{
    public abstract float CalculateUtility(GodContext context);
    public abstract void Execute(GodContext context);
}
