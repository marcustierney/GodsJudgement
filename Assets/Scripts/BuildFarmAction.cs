using UnityEngine;

public class BuildFarmAction : VillageAction
{
    public float woodCost = 50f;
    public GameObject farmPrefab;

    public override float CalculateUtility(VillageContext context)
    {
        if (context.wood < woodCost)
        {
            return 0;
        }
        float needFood = 1f - (context.food / 100f); //prefer building farms if food is low
        return Mathf.Clamp01(needFood);
    }

    public override void Execute(VillageContext context)
    {
        if (!ResourceManager.Instance.SpendWood(woodCost))
        {
            return;
        }
        Vector3 pos = FindValidFarmPosition(context.townHall);
        Object.Instantiate(farmPrefab, pos, Quaternion.identity);
    }

    private Vector3 FindValidFarmPosition(GameObject townHall)
    {
        float townHallHalfX = townHall.transform.localScale.x / 2f; 
        float townHallHalfZ = townHall.transform.localScale.z / 2f; 
        float farmHalfX = 4f / 2f; 
        float farmHalfZ = 4f / 2f; 
        float clearanceX = townHallHalfX + farmHalfX + 0.5f; 
        float clearanceZ = townHallHalfZ + farmHalfZ + 0.5f; 
        Vector3 center = townHall.transform.position;
        float spawnRadius = 12f; 
        for (int i = 0; i < 30; i++) //try random positions until one is clear
        {
            float x = Random.Range(-spawnRadius, spawnRadius);
            float z = Random.Range(-spawnRadius, spawnRadius);
            if (Mathf.Abs(x) < clearanceX && Mathf.Abs(z) < clearanceZ)
                continue;

            return new Vector3(center.x + x, 1f, center.z + z);
        }
        return center + new Vector3(spawnRadius, 1f, 0);
    }
}