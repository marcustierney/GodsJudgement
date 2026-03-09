using UnityEngine;

public class Turret : MonoBehaviour
{
    public float range = 10f;
    public float damage = 20f;
    public float fireRate = 1f; 
    private float fireCooldown = 0f;
    public GameObject arrowPrefab;
    public Transform arrowSpawnPoint;
    private GameObject currentTarget;
    public AudioClip arrowSound;
    private AudioSource audioSource;
    private void Start()
    {
        transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
        Vector3 pos = transform.position;
        pos.y = -0.2f;
        transform.position = pos;
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        fireCooldown -= Time.deltaTime;
        currentTarget = FindNearestEnemy();
        if (currentTarget != null && fireCooldown <= 0f)
        {
            ShootArrow();
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
            Health h = enemy.GetComponent<Health>();
            if (h != null && h.isDead)
            {
                continue;
            }
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < closestDist && dist <= range)
            {
                closestDist = dist;
                nearest = enemy;
            }
        }

        return nearest;
    }

    void ShootArrow()
    {
        if (currentTarget == null)
        {
            return;
        }
        Vector3 targetPos;
        Collider enemyCollider = currentTarget.GetComponent<Collider>();
        targetPos = enemyCollider.bounds.center;
        Vector3 dir = (targetPos - arrowSpawnPoint.position).normalized;
        GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.identity);
        Arrow arrowScript = arrow.GetComponent<Arrow>();
        audioSource.PlayOneShot(arrowSound);
        arrowScript.Launch(dir);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}