using UnityEngine;

public class GruntEnemy : EnemyBase
{
    protected override void HandleAI()
    {
        if (target == null) return;

        float dist = Vector2.Distance(transform.position, target.position);
        if (dist <= stats.attackRange) Attack();
        else MoveTowardsTarget();
    }
}
