using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    UnitMovement movement;
    UnitCombat combat;
    GameObject townHall;
    Health myHealth;
    private bool isCollidingWithTownHall = false;

    void Start()
    {
        movement = GetComponent<UnitMovement>();
        combat = GetComponent<UnitCombat>();
        townHall = GameManager.Instance.townHall;
        myHealth = GetComponent<Health>();
    }

    void Update()
    {
        if (myHealth != null && myHealth.isDead)
        {
            return;
        }
        if (townHall == null)
        {
            return;
        }
        if (!isCollidingWithTownHall)
        {
            movement.MoveTo(townHall.transform.position);
        }
        else
        {
            combat.TryAttack(townHall);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == townHall)
        {
            isCollidingWithTownHall = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == townHall)
        {
            isCollidingWithTownHall = false;
        }
    }
}