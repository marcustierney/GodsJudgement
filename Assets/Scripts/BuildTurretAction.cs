using UnityEngine;

public class BuildTurretAction : VillageAction
{
    public float woodCost = 80f;
    public GameObject turretPrefab;

    public override float CalculateUtility(VillageContext context)
    {
        return 0;
    }

    public override void Execute(VillageContext context) { }
}