using UnityEngine;

public class UnitCombat : MonoBehaviour
{
    public float damage = 10;
    public float attackRange = 2f;
    public float attackCooldown = 1f;
    float cooldownTimer;

    public bool TryAttack(GameObject target)
    {
        if (target == null)
        {
            return false;
        }
        cooldownTimer -= Time.deltaTime;
        float dist = Vector3.Distance(transform.position, target.transform.position);
        if (dist > attackRange)
        {
            return false;
        }
        if (cooldownTimer > 0)
        {
            return true;
        }
        Health health = target.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }
        cooldownTimer = attackCooldown;
        return true;
    }
}