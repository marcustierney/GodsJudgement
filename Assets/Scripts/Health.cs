using UnityEngine;
using System;
using UnityEngine.SceneManagement;

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
        TroopStats stats = GetComponent<TroopStats>();
        if (stats != null)
        {
            amount = stats.ModifyIncomingDamage(amount);
        }
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (isDead)
        {
            return;
        }
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    void Die()
    {
        if (isDead)
        {
            return;
        }
        isDead = true;
        TroopAnimator troopAnim = GetComponent<TroopAnimator>();
        if (troopAnim != null)
        {
            troopAnim.TriggerDeath();
        }
        else
        {
            EnemyAnimator enemyAnim = GetComponent<EnemyAnimator>();
            if (enemyAnim != null)
            {
                enemyAnim.TriggerDeath();
            }
        }
        if (CompareTag("Friendly"))
        {
            GodManager.Instance.RegisterTroopDeath();
        }
        if (CompareTag("Enemy"))
        {
            GodManager.Instance.RegisterEnemyDeath();
        }
        if (CompareTag("TownHall"))
        {
            TriggerGameOver();
        }
        if (OnDeath != null)
        {
            OnDeath.Invoke(gameObject);
        }
        Destroy(gameObject, 1.5f);
    }

    void TriggerGameOver()
    {
        GodPersonality p = GodManager.Instance.GetPersonality();
        GameOverData.Instance.moralitySatisfaction = p.moralitySatisfaction;
        GameOverData.Instance.styleSatisfaction = p.styleSatisfaction;
        GameOverData.Instance.consumptionSatisfaction = p.consumptionSatisfaction;
        GameOverData.Instance.morality = p.morality;
        GameOverData.Instance.style = p.style;
        GameOverData.Instance.consumption = p.consumption;
        Debug.Log($"Saved stats - Morality: {p.moralitySatisfaction} Style: {p.styleSatisfaction} Consumption: {p.consumptionSatisfaction}");
        GameOverData.Instance.wavesReached = WaveSpawner.Instance.currentWave;
        SceneManager.LoadScene("LoseScene");
    }
}