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
        Collider targetCollider = target.GetComponent<Collider>();
        Vector3 closestPoint;
        if (targetCollider != null)
        {
            closestPoint = targetCollider.ClosestPoint(transform.position);
        }
        else
        {
            closestPoint = target.transform.position;
        }
        float dist = Vector3.Distance(transform.position, closestPoint);
        if (dist > attackRange)
        {
            return false;
        }
        if (cooldownTimer > 0)
        {
            return true;
        }
        Health health = target.GetComponent<Health>();
        if (health != null && !health.isDead)
        {
            health.TakeDamage(damage);
            if (health.isDead && GodManager.Instance != null && CompareTag("Friendly"))
            {
                GodManager.Instance.RegisterTroopKill();
            }
            cooldownTimer = attackCooldown;
            return true;
        }
        BuildingHealth buildingHealth = target.GetComponent<BuildingHealth>();
        if (buildingHealth != null)
        {
            buildingHealth.TakeDamage(damage);
            cooldownTimer = attackCooldown;
            return true;
        }
        cooldownTimer = attackCooldown;
        Wall wall = target.GetComponent<Wall>();
        if (wall != null)
        {
            wall.TakeDamage(damage);
            cooldownTimer = attackCooldown;
            return true;
        }
        cooldownTimer = attackCooldown;
        return true;
    }
}