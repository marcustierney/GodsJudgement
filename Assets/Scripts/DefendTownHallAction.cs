using UnityEngine;
public class DefendTownHallAction : UtilityAction
{
    public float defendRadius = 8f; 

    public override float CalculateUtility(AIContext context)
    {
        if (context.nearestEnemy == null)
        {
            return 0;
        }
        if (context.distanceToTownHall < defendRadius)
        {
            return 0;
        }
        if (context.nearestEnemyDistance > 15f)
        {
            return 0;
        }
        float urgency = context.nearestEnemyDistance / 15f; //score drops as troop gets closer to townhall so it naturally hands off to attack
        return 2f + urgency;
    }

    public override void Execute(AIContext context)
    {
        UnitMovement movement = context.self.GetComponent<UnitMovement>();

        Vector3 townHallPos = context.townHall.transform.position;
        Vector3 selfPos = context.self.transform.position;
        //stop moving if already within 10 units on X and Z
        float dx = Mathf.Abs(selfPos.x - townHallPos.x);
        float dz = Mathf.Abs(selfPos.z - townHallPos.z);
        if (dx < 10f && dz < 10f)
        {
            return;
        }
        Vector3 dirAway = (selfPos - townHallPos); //move to edge of townhall rather than its center
        dirAway.y = 0;
        if (dirAway.sqrMagnitude < 0.001f)
        {
            dirAway = Vector3.forward;
        }
        dirAway.Normalize();
        float stopDistance = 4f;
        Vector3 targetPosition = townHallPos + dirAway * stopDistance;
        movement.MoveTo(targetPosition);
    }
}