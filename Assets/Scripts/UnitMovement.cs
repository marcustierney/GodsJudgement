using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class UnitMovement : MonoBehaviour
{
    public float speed = 3f;
    public float stoppingDistance = 0.2f;
    private Rigidbody rb;
    private Vector3? currentTarget = null;
    public float lockedY = 0.5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        rb.useGravity = false;
    }

    void FixedUpdate()
    {
        if (rb.position.y != lockedY)
        {
            rb.position = new Vector3(rb.position.x, 0.5f, rb.position.z);
        }

        if (currentTarget == null)
        {
            return;
        }

        Vector3 dir = currentTarget.Value - rb.position;
        dir.y = 0;

        if (dir.sqrMagnitude <= stoppingDistance * stoppingDistance)
        {
            StopMoving();
            return;
        }

        dir.Normalize();
        Quaternion targetRotation = Quaternion.LookRotation(dir);
        rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, 10f * Time.fixedDeltaTime);
        Vector3 newPos = rb.position + dir * speed * Time.fixedDeltaTime;
        newPos.y = 0.5f;
        rb.MovePosition(newPos);
    }

    public void MoveTo(Vector3 target)
    {
        currentTarget = new Vector3(target.x, rb.position.y, target.z);
    }

    public void StopMoving()
    {
        currentTarget = null;
        rb.linearVelocity = Vector3.zero;
    }

    public bool HasReachedTarget()
    {
        if (currentTarget == null)
        {
            return true;
        }
        Vector3 dir = currentTarget.Value - rb.position;
        dir.y = 0;
        float squaredDistance = dir.sqrMagnitude;
        float squaredStoppingDistance = stoppingDistance * stoppingDistance;
        if (squaredDistance <= squaredStoppingDistance)
        {
            return true;
        }
        return false;
    }
}