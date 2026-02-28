using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public float speed = 5f;

    public void MoveTo(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        dir.y = 0;
        dir.Normalize();
        transform.position += dir * speed * Time.deltaTime;
    }
}