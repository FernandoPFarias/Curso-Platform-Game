using UnityEngine;

[CreateAssetMenu(menuName = "AI Behaviours/Patrol Chase Attack")]
public class PatrolChaseAttackBehaviour : AIBehaviour
{
    private enum State { Patrolling, Alert, Chasing, Knockback, Attacking }
    private State currentState;

    private Transform currentPatrolTarget;
    private float stateTimer;

    public override void Initialize(Enemy puppet)
    {
        ChangeState(State.Patrolling, puppet);
    }

    public override void OnTakeDamage(Enemy puppet)
    {
        ChangeState(State.Knockback, puppet);
    }

    public override void Tick(Enemy puppet)
    {
        if (puppet.PlayerTarget == null) return;

        switch (currentState)
        {
            case State.Patrolling:
                if (IsPlayerDetected(puppet))
                {
                    ChangeState(State.Alert, puppet);
                    break;
                }
                puppet.MoveTowards(currentPatrolTarget.position);
                break;

            case State.Alert:
                stateTimer -= Time.fixedDeltaTime;
                if (stateTimer <= 0)
                {
                    ChangeState(State.Chasing, puppet);
                }
                break;

            case State.Chasing:
                if (Vector2.Distance(puppet.transform.position, puppet.PlayerTarget.position) > puppet.EnemyData.giveUpRange)
                {
                    ChangeState(State.Patrolling, puppet);
                    break;
                }
                puppet.MoveTowards(puppet.PlayerTarget.position);
                break;

            case State.Knockback:
                stateTimer -= Time.fixedDeltaTime;
                if (stateTimer <= 0)
                {
                    ChangeState(State.Chasing, puppet);
                }
                break;
        }
    }

    private void ChangeState(State newState, Enemy puppet)
    {
        currentState = newState;

        // AGORA CHAMAMOS OS MÉTODOS DO NOSSO CONTROLADOR DE ANIMAÇÃO
        switch (currentState)
        {
            case State.Patrolling:
                puppet.AnimationManager?.TriggerWalk();
                currentPatrolTarget = (currentPatrolTarget == puppet.PointA) ? puppet.PointB : puppet.PointA;
                break;
            case State.Alert:
                puppet.Rb.linearVelocity = Vector2.zero;
                puppet.AnimationManager?.TriggerAlert();
                if (puppet.PlayerTarget != null) puppet.FlipTowards(puppet.PlayerTarget.position);
                stateTimer = puppet.EnemyData.alertDuration;
                break;
            case State.Chasing:
                puppet.AnimationManager?.TriggerChase();
                break;
            case State.Knockback:
                stateTimer = puppet.EnemyData.knockbackDuration;
                puppet.AnimationManager?.TriggerHurt();
                // ... (lógica de física do knockback)
                break;
        }
    }

    // A lógica de detecção continua a mesma
    private bool IsPlayerDetected(Enemy puppet)
    {
        return Vector2.Distance(puppet.transform.position, puppet.PlayerTarget.position) < puppet.EnemyData.detectionRange;
    }
}