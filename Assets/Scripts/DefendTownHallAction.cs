using UnityEngine;
public class DefendTownHallAction : UtilityAction
{
    public float defendRadius = 8f;
    public float stopDistance = 2f; 

    public override float CalculateUtility(AIContext context)
    {
        if (context.nearestEnemy == null)
        {
            return 0;
        }
        if (context.distanceToTownHall < defendRadius) //Only pull troops back if drifted too far from townhall
        {
            return 0;
        }
        if (context.nearestEnemyDistance > 15f)
        {
            return 0;
        }
        float urgency = context.nearestEnemyDistance / 15f;
        return 2f + urgency;
    }

    public override void Execute(AIContext context)
    {
        UnitMovement movement = context.self.GetComponent<UnitMovement>();
        Vector3 townHallPos = context.townHall.transform.position;
        Vector3 selfPos = context.self.transform.position;
        Vector3 dirAway = selfPos - townHallPos;
        dirAway.y = 0;
        if (dirAway.sqrMagnitude < 0.001f)
        {
            dirAway = Vector3.forward;
        }
        dirAway.Normalize();
        float townHallRadius = 0f;
        Collider townHallCollider = context.townHall.GetComponent<Collider>();
        if (townHallCollider != null)
        {
            Vector3 closestPoint = townHallCollider.ClosestPoint(selfPos);
            townHallRadius = Vector3.Distance(townHallPos, closestPoint);
        }
        else
        {
            townHallRadius = Mathf.Max(context.townHall.transform.localScale.x, context.townHall.transform.localScale.z) / 2f;
        }
        Vector3 targetPos = townHallPos + dirAway * (townHallRadius + stopDistance);
        float distToTarget = Vector3.Distance(new Vector3(selfPos.x, 0, selfPos.z), new Vector3(targetPos.x, 0, targetPos.z));
        if (distToTarget < 0.5f)
        {
            movement.StopMoving();
            return;
        }
        movement.MoveTo(targetPos);
    }
}