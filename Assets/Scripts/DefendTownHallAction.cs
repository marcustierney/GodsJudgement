using UnityEngine;

public class DefendTownHallAction : UtilityAction
{
    public override float CalculateUtility(AIContext context)
    {
        if (context.nearestEnemy == null)
        {
            return 0;
        }
        if (context.nearestEnemyDistance > 15)
        {
            return 0;
        }
        return 5;
    }
    public override void Execute(AIContext context)
    {
        UnitMovement movement = context.self.GetComponent<UnitMovement>();
        movement.MoveTo(context.townHall.transform.position);
    }
}