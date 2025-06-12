using UnityEngine;

public class PatrolState : AIState
{
    private Transform currentPatrolTarget;

    public PatrolState(Enemy enemy): base(enemy) { }


    public override void OnEnter()
    {
        Debug.Log("Entrando no estado de Patrulha.");
        // Define o primeiro alvo da patrulha

        currentPatrolTarget = stateMachine.pointA;

    }

    public override void Tick()
    {
        // Se o jogador entrar no raio de detecção, muda para o estado de Perseguição.
        if (Vector2.Distance(stateMachine.transform.position, stateMachine.PlayerTarget.position) < stateMachine.EnemyData.detectionRange)
        {
            stateMachine.ChangeState(new ChaseState(stateMachine));
            return;
        }

        // Lógica de patrulha
        stateMachine.MoveTowards(currentPatrolTarget.position);
        if (Vector2.Distance(stateMachine.transform.position, currentPatrolTarget.position) < 0.5f)
        {
            currentPatrolTarget = (currentPatrolTarget == stateMachine.PointA) ? stateMachine.PointB : stateMachine.PointA;
        }
    }


}
