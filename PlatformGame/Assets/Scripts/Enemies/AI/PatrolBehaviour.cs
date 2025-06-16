using UnityEngine;

[CreateAssetMenu(menuName = "AI Behaviours/Patrol then Chase")]
public class PatrolBehaviour : AIBehaviour
{
    // A máquina de estados agora vive DENTRO do comportamento!
    private enum State { Patrolling, Alert, Chasing }
    private State currentState;

    private Transform currentPatrolTarget;
    private float stateTimer;

    // Initialize é chamado pelo Enemy.cs quando o inimigo é criado.
    public override void Initialize(Enemy puppet)
    {
        currentState = State.Patrolling;
        currentPatrolTarget = puppet.PointA;
        if (puppet.Animator != null) puppet.Animator.SetTrigger("T_StartChase");
    }

    // Tick é chamado a cada FixedUpdate pelo Enemy.cs
    public override void Tick(Enemy puppet)
    {
        switch (currentState)
        {
            case State.Patrolling:
                // Se detectar o jogador, muda para o estado de Alerta
                if (IsPlayerDetected(puppet))
                {
                    currentState = State.Alert;
                    stateTimer = puppet.EnemyData.alertDuration;
                    puppet.Rb.linearVelocity = Vector2.zero;
                    if (puppet.Animator != null) puppet.Animator.SetTrigger("GoToIdle");
                    if (puppet.PlayerTarget != null) puppet.FlipTowards(puppet.PlayerTarget.position);
                    break;
                }

                // Lógica de Patrulha
                puppet.MoveTowards(currentPatrolTarget.position);
                if (Vector2.Distance(puppet.transform.position, currentPatrolTarget.position) < 0.5f)
                {
                    currentPatrolTarget = (currentPatrolTarget == puppet.PointA) ? puppet.PointB : puppet.PointA;
                }
                break;

            case State.Alert:
                stateTimer -= Time.fixedDeltaTime;
                if (stateTimer <= 0)
                {
                    currentState = State.Chasing;
                    if (puppet.Animator != null) puppet.Animator.SetTrigger("T_StartPatrol");
                }
                break;

            case State.Chasing:
                if (puppet.PlayerTarget == null)
                {
                    currentState = State.Patrolling;
                    if (puppet.Animator != null) puppet.Animator.SetTrigger("T_StartPatrol");
                    break;
                }

                // Lógica de perseguição...
                puppet.MoveTowards(puppet.PlayerTarget.position);

                // ... com a condição de desistir se o jogador for longe demais
                if (Vector2.Distance(puppet.transform.position, puppet.PlayerTarget.position) > puppet.EnemyData.giveUpRange)
                {
                    currentState = State.Patrolling;
                    if (puppet.Animator != null) puppet.Animator.SetTrigger("T_StartPatrol");
                }
                break;
        }
    }

    public override bool IsPlayerDetected(Enemy puppet)
    {
        if (puppet.PlayerTarget == null) return false;

        float distanceToPlayer = Vector2.Distance(puppet.transform.position, puppet.PlayerTarget.position);
        return distanceToPlayer < puppet.EnemyData.detectionRange;
    }
}