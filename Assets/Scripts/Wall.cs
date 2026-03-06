using UnityEngine;

public class Wall : MonoBehaviour
{
    public float maxHealth = 150f;
    public float currentHealth;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
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
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 2f);
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
