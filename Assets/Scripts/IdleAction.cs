public class IdleAction : UtilityAction
{
    public override float CalculateUtility(AIContext context)
    {
        return 0.1f;
    }

    public override void Execute(AIContext context)
    {
        //do nothing
    }
}