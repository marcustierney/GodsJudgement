using UnityEngine;

public class Barracks : MonoBehaviour
{
    public GameObject troopPrefab;
    public float spawnInterval = 2f;
    private float spawnTimer;
    public float healthDecayPerSecond = 5f;
    private BuildingHealth buildingHealth;

    void Start()
    {
        buildingHealth = GetComponent<BuildingHealth>();
    }

    void Update()
    {
        if (buildingHealth == null || buildingHealth.currentHealth <= 0)
        {
            return;
        }
        buildingHealth.TakeDamage(healthDecayPerSecond * Time.deltaTime);
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0;
            SpawnTroop();
        }
    }

    void SpawnTroop()
    {
        if (!ResourceManager.Instance.CanCreateTroop(1))
        {
            Debug.Log("Troop cap reached");
            return;
        }
        Vector3 spawnOffset = new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
        Instantiate(troopPrefab, transform.position + spawnOffset, Quaternion.identity);
        ResourceManager.Instance.RegisterTroop();
    }
}