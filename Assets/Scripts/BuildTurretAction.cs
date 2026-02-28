using UnityEngine;

public class BuildTurretAction : VillageAction
{
    public float woodCost = 80f;
    public GameObject turretPrefab;

    public override float CalculateUtility(VillageContext context)
    {
        if (context.wood < woodCost)
        {
            return 0;
        }
        return Mathf.Clamp01(1f - ((float)context.turretCount / 5f)); //slightly prefer building turrets if we have few
    }

    public override void Execute(VillageContext context)
    {
        if (!ResourceManager.Instance.SpendWood(woodCost))
        {
            return;
        }
        Vector3 pos = context.townHall.transform.position + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
        Object.Instantiate(turretPrefab, pos, Quaternion.identity);
    }
}