using UnityEngine;
public class BuildFarmAction : VillageAction
{
    public float woodCost = 50f;
    public GameObject farmPrefab;

    public override float CalculateUtility(VillageContext context)
    {
        return 0;
    }

    public override void Execute(VillageContext context) { }
}