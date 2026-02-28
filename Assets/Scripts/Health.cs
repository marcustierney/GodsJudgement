using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;

    public Action<GameObject> OnDeath;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        OnDeath?.Invoke(gameObject);
        Destroy(gameObject);
    }
}