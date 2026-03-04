using UnityEngine;
using UnityEngine.InputSystem;

public class Barracks : MonoBehaviour
{
    public GameObject troopPrefab;
    public float spawnInterval = 2f;
    private float spawnTimer;
    public float maxHealth = 100f;
    public float currentHealth;
    public float healthDecayPerSecond = 5f;
    public float foodHealAmount = 25f;
    public float foodCostPerFeed = 10f;

    private bool isDead = false;
    private Camera mainCam;

    void Start()
    {
        currentHealth = maxHealth;
        mainCam = Camera.main;
    }

    void Update()
    {
        if (isDead)
        {
            return;
        }
        currentHealth -= healthDecayPerSecond * Time.deltaTime;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
            return;
        }

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0;
            SpawnTroop();
        }
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            CheckClick();
        }
    }

    void CheckClick()
    {
        if (BuildingPlacer.Instance != null && BuildingPlacer.Instance.IsPlacing())
        {
            return;
        }
        Ray ray = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, 200f))
        {
            if (hit.collider.gameObject == gameObject)
            {
                FeedBarracks();
            }
        }
    }

    void FeedBarracks()
    {
        if (!ResourceManager.Instance.HasFood(foodCostPerFeed))
        {
            Debug.Log("Not enough food");
            return;
        }
        ResourceManager.Instance.SpendFood(foodCostPerFeed);
        currentHealth = Mathf.Min(currentHealth + foodHealAmount, maxHealth);
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

    void Die()
    {
        isDead = true;
        Destroy(gameObject);
    }

    void OnGUI()
    {
        if (isDead) 
        {
            return;

        }
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 3f);
        if (screenPos.z < 0)
        {
            return;
        }
        float barWidth = 60f;
        float barHeight = 8f;
        float x = screenPos.x - barWidth / 2f;
        float y = Screen.height - screenPos.y - barHeight / 2f;
        GUI.color = Color.black;
        GUI.DrawTexture(new Rect(x - 1, y - 1, barWidth + 2, barHeight + 2), Texture2D.whiteTexture);
        float healthPercent = currentHealth / maxHealth;
        GUI.color = Color.Lerp(Color.red, Color.green, healthPercent);
        GUI.DrawTexture(new Rect(x, y, barWidth * healthPercent, barHeight), Texture2D.whiteTexture);
        GUI.color = Color.white;
    }
}