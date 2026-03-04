using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
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
                FeedBuilding();
            }       
        }
    }

    void FeedBuilding()
    {
        if (!ResourceManager.Instance.HasFood(foodCostPerFeed))
        {
            Debug.Log("not enough food to heal");
            {
                return;
            }
            
        }
        ResourceManager.Instance.SpendFood(foodCostPerFeed);
        currentHealth = Mathf.Min(currentHealth + foodHealAmount, maxHealth);
        Debug.Log($"{gameObject.name} healed to {currentHealth:F0}/{maxHealth}");
    }

    public void TakeDamage(float amount)
    {
        if (isDead)
        {
            return;
        }
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    void Die()
    {
        if (isDead)
        {
            return;
        }
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