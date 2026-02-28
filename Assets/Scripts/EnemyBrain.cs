using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    UnitMovement movement;
    UnitCombat combat;
    GameObject townHall;

    void Start()
    {
        movement = GetComponent<UnitMovement>();
        combat = GetComponent<UnitCombat>();
        townHall = GameManager.Instance.townHall;
    }

    void Update()
    {
        bool inRange = combat.TryAttack(townHall);
        if (!inRange)
        {
            movement.MoveTo(townHall.transform.position);
        }
    }
}