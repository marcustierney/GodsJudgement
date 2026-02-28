using Unity;

public abstract class UtilityAction
{
    public abstract float CalculateUtility(AIContext context);

    public abstract void Execute(AIContext context);
}