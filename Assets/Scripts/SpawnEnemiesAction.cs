using UnityEngine;

public class SpawnEnemiesAction : GodAction
{
    public override float CalculateUtility(GodContext context)
    {
        float baseValue = 0;
        if (context.enemyCount < context.troopCount)
        {
            baseValue = 0.5f;
        }
        return baseValue * (1 + context.aggression);
    }

    public override void Execute(GodContext context)
    {
        EnemySpawner spawner = GameObject.FindObjectOfType<EnemySpawner>();
        if (spawner != null)
        {
            spawner.SpawnEnemies(10);
        }

        Debug.Log("God spawned more enemies");
    }
}