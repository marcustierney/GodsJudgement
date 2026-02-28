using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class UnitMovement : MonoBehaviour
{
    public float speed = 3f;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        rb.useGravity = false;
    }


    public void MoveTo(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        dir.y = 0;
        if (dir.sqrMagnitude < 0.001f)
        {
            return;
        }
        dir.Normalize();
        Vector3 newPos = rb.position + dir * speed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);
    }
}