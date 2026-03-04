using UnityEngine;

public class Turret : MonoBehaviour
{
    public float range = 10f;
    public float damage = 20f;
    public float fireRate = 1f; 

    private float fireCooldown = 0f;

    void Update()
    {
        fireCooldown -= Time.deltaTime;

        GameObject target = FindNearestEnemy();
        if (target != null && fireCooldown <= 0f)
        {
            Attack(target);
            fireCooldown = 1f / fireRate;
        }
    }

    GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearest = null;
        float closestDist = Mathf.Infinity;

        foreach (var enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < closestDist && dist <= range)
            {
                closestDist = dist;
                nearest = enemy;
            }
        }

        return nearest;
    }

    void Attack(GameObject enemy)
    {
        if (enemy == null) return;

        Health health = enemy.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(damage);
            Debug.DrawLine(transform.position + Vector3.up * 1.5f, enemy.transform.position + Vector3.up * 1f, Color.red, 0.1f, false);
            print("enemy hit");
        }
    }

    void OnDrawGizmosSelected()
    {
        //visualize turret range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}