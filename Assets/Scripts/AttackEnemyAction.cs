using UnityEngine;
public class AttackEnemyAction : UtilityAction
{
    public override float CalculateUtility(AIContext context)
    {
        if (context.nearestEnemy == null)
        {
            return 0;
        }
        float healthPercent = context.health / context.maxHealth;
        float distanceScore = 1f / (context.nearestEnemyDistance + 1);
        return healthPercent * distanceScore * 10;
    }

    public override void Execute(AIContext context)
    {
        if (context.nearestEnemy == null)
        {
            return;
        }
        UnitMovement movement = context.self.GetComponent<UnitMovement>();
        UnitCombat combat = context.self.GetComponent<UnitCombat>();

        bool inRange = combat.TryAttack(context.nearestEnemy);
        if (!inRange)
        {
            movement.MoveTo(context.nearestEnemy.transform.position);
        }
        else
        {
            movement.StopMoving();
        }
    }
}