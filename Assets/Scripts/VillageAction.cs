using Unity;
public abstract class VillageAction
{
    public abstract float CalculateUtility(VillageContext context);
    public abstract void Execute(VillageContext context);
}