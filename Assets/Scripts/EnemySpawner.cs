using UnityEngine;
public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float enemyRadius = 0.5f;
    public int maxAttemptsPerEnemy = 15;

    public void SpawnEnemies(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3? spawnPos = FindValidSpawnPosition();
            if (spawnPos == null)
            {
                continue;
            }
            Instantiate(enemyPrefab, spawnPos.Value, Quaternion.identity);
        }
    }

    Vector3? FindValidSpawnPosition()
    {
        Transform[] shuffled = ShuffledSpawnPoints();

        foreach (Transform spawnPoint in shuffled)
        {
            for (int attempt = 0; attempt < maxAttemptsPerEnemy; attempt++)
            {
                Vector2 randomOffset = Random.insideUnitCircle * 2f; //Spread enemies randomly around spawn point
                Vector3 candidate = spawnPoint.position + new Vector3(randomOffset.x, 0, randomOffset.y);

                if (!IsOccupied(candidate))
                    return candidate;
            }
        }
        return null;
    }

    bool IsOccupied(Vector3 pos)
    {
        Collider[] hits = Physics.OverlapSphere(pos, enemyRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy") || hit.CompareTag("Building") || hit.CompareTag("TownHall"))
            {
                return true;
            }
        }
        return false;
    }

    Transform[] ShuffledSpawnPoints()
    {
        Transform[] copy = (Transform[])spawnPoints.Clone();
        for (int i = copy.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            var temp = copy[i];
            copy[i] = copy[j];
            copy[j] = temp;
        }
        return copy;
    }
}