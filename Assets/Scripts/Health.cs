using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;
    private bool _isDead;
    Health health;
    public Action<GameObject> OnDeath;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public bool isDead
    {
        get { return _isDead; }
        private set { _isDead = value; }
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
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;
        isDead = true;
        if (CompareTag("Friendly"))
        {
            GodManager.Instance.RegisterTroopDeath();
        }
        if (CompareTag("Enemy"))
        {
            GodManager.Instance.RegisterEnemyDeath();
        }
        if (OnDeath != null)
        {
            OnDeath.Invoke(gameObject);
        }
        Destroy(gameObject);
    }
}