using UnityEngine;

public class DasherEnemy : EnemyBase
{
    private enum DasherState { Chase, Windup, Dash, Cooldown }

    private DasherState state = DasherState.Chase;
    private float stateTimer;
    private Vector2 dashDirection;
    private readonly float windupDuration = 0.5f;

    protected override void HandleAI()
    {
        if (target == null) return;

        float dist = Vector2.Distance(transform.position, target.position);

        switch (state)
        {
            case DasherState.Chase:
                if (dist <= stats.attackRange * 3f && attackTimer <= 0f)
                {
                    state = DasherState.Windup;
                    stateTimer = windupDuration;
                    rb.linearVelocity = Vector2.zero;
                    dashDirection = ((Vector2)target.position - (Vector2)transform.position).normalized;
                    if (spriteRenderer != null) spriteRenderer.color = Color.yellow;
                }
                else
                {
                    MoveTowardsTarget();
                }
                break;

            case DasherState.Windup:
                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0f)
                {
                    state = DasherState.Dash;
                    stateTimer = stats.dashDuration;
                    if (spriteRenderer != null) spriteRenderer.color = Color.white;
                }
                break;

            case DasherState.Dash:
                rb.linearVelocity = dashDirection * stats.dashSpeed;
                stateTimer -= Time.deltaTime;
                if (dist <= stats.attackRange) Attack();

                if (stateTimer <= 0f)
                {
                    state = DasherState.Cooldown;
                    stateTimer = stats.dashCooldown;
                    rb.linearVelocity = Vector2.zero;
                    attackTimer = stats.attackCooldown;
                }
                break;

            case DasherState.Cooldown:
                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0f)
                    state = DasherState.Chase;
                break;
        }
    }
}
