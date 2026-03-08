using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float damage = 40f;
    public float lifetime = 5f;
    public float speed = 15f;
    private bool hasHit = false;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (rb.linearVelocity.sqrMagnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(rb.linearVelocity);
        }
    }

    public void Launch(Vector3 direction)
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = direction.normalized * speed;
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;

        if (other.CompareTag("Friendly") || other.CompareTag("Building") || other.CompareTag("TownHall") || other.CompareTag("Tree"))
        {
            return;
        }

        if (other.CompareTag("Enemy"))
        {
            Health health = other.GetComponent<Health>();
            if (health != null && !health.isDead)
            {
                health.TakeDamage(damage);
                hasHit = true;
                Destroy(gameObject);
            }
        }
    }
}