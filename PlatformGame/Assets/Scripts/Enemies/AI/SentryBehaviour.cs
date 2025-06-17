using UnityEngine;

[CreateAssetMenu(menuName = "AI Behaviours/Sentry Behaviour")]
public class SentryBehaviour : AIBehaviour
{
    private enum State { Idling, Alert, Chasing, Knockback }
    private State currentState;

    private float stateTimer;

    public override void Initialize(Enemy puppet)
    {
        ChangeState(State.Idling, puppet);
    }

    public override void OnTakeDamage(Enemy puppet)
    {
        ChangeState(State.Knockback, puppet);
    }

    public override void Tick(Enemy puppet)
    {
        if (puppet.PlayerTarget == null && currentState != State.Idling)
        {
            ChangeState(State.Idling, puppet);
            return;
        }

        switch (currentState)
        {
            case State.Idling:
                if (IsPlayerDetected(puppet))
                {
                    ChangeState(State.Alert, puppet);
                }
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
                    ChangeState(State.Idling, puppet);
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

        // AGORA USANDO OS MÉTODOS DO NOSSO ENEMYANIMATOR
        switch (currentState)
        {
            case State.Idling:
                puppet.Rb.linearVelocity = Vector2.zero;
                puppet.AnimationManager?.TriggerIdle();
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

                if (puppet.PlayerTarget != null)
                {
                    Vector2 direction = (puppet.transform.position - puppet.PlayerTarget.position).normalized;
                    Vector2 force = new Vector2(direction.x * puppet.EnemyData.knockbackForce.x, puppet.EnemyData.knockbackForce.y);
                    puppet.Rb.linearVelocity = Vector2.zero;
                    puppet.Rb.AddForce(force, ForceMode2D.Impulse);
                }
                break;
        }
    }

    // A lógica de detecção por Raycast continua a mesma
    private bool IsPlayerDetected(Enemy puppet)
    {
        if (puppet.PlayerTarget == null) return false;
        Vector2 directionToPlayer = puppet.PlayerTarget.position - puppet.transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;
        if (distanceToPlayer > puppet.EnemyData.detectionRange) return false;

        float facingDirection = puppet.startsFacingLeft ? -puppet.transform.localScale.x : puppet.transform.localScale.x;
        float playerDirection = Mathf.Sign(directionToPlayer.x);
        if (Mathf.Abs(facingDirection - playerDirection) > 0.1f) return false;

        RaycastHit2D hit = Physics2D.Raycast(puppet.transform.position, directionToPlayer.normalized, distanceToPlayer, LayerMask.GetMask("Player", "Ground"));
        return hit.collider != null && hit.collider.CompareTag("Player");
    }
}