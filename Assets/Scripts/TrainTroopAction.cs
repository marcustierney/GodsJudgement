using UnityEngine;

public class TrainTroopAction : VillageAction
{
    public float foodCost = 20f;
    public GameObject troopPrefab;

    public override float CalculateUtility(VillageContext context)
    {
        if (!ResourceManager.Instance.CanCreateTroop((int)foodCost))
        {
            return 0;
        }
        float needTroops = 1f - ((float)context.currentTroops / context.maxTroops);
        return needTroops; //higher score if we have few troops
    }

    public override void Execute(VillageContext context)
    {
        if (!ResourceManager.Instance.SpendFood(foodCost))
        {
            return;
        }
        Vector3 pos = FindValidTroopPosition(context.townHall);
        GameObject troop = Object.Instantiate(troopPrefab, pos, Quaternion.identity);
        ResourceManager.Instance.RegisterTroop();
    }

    private Vector3 FindValidTroopPosition(GameObject townHall)
    {
        float townHallHalfX = townHall.transform.localScale.x / 2f; 
        float townHallHalfZ = townHall.transform.localScale.z / 2f; 
        float troopRadius = 0.5f;
        float clearanceX = townHallHalfX + troopRadius + 0.5f;
        float clearanceZ = townHallHalfZ + troopRadius + 0.5f; 
        Vector3 center = townHall.transform.position;
        float spawnRadius = 8f;

        for (int i = 0; i < 30; i++)
        {
            float x = Random.Range(-spawnRadius, spawnRadius);
            float z = Random.Range(-spawnRadius, spawnRadius);

            if (Mathf.Abs(x) < clearanceX && Mathf.Abs(z) < clearanceZ)
                continue;

            return new Vector3(center.x + x, 1f, center.z + z);
        }
        return center + new Vector3(clearanceX + 1f, 1f, 0); //fallback to spawn directly to the side of the townhall
    }
}