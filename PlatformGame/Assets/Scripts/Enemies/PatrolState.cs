using UnityEngine;

public class PatrolState : AIState
{
    private Transform currentPatrolTarget;

    public PatrolState(Enemy enemy) : base(enemy) { }

    public override void OnEnter()
    {

        
        // Define o primeiro alvo da patrulha.
        currentPatrolTarget = stateMachine.PointA;
        



    }

    public override void Tick()
    {
        // CONDIÇÃO DE TRANSIÇÃO: O jogador está perto o suficiente?
        if (Vector2.Distance(stateMachine.transform.position, stateMachine.PlayerTarget.position) < stateMachine.EnemyData.detectionRange)
        {
            // Se sim, avisa o gerente para contratar o especialista de ALERTA.
            stateMachine.ChangeState(new AlertState(stateMachine));
            return;
        }

        // Lógica de patrulha normal...
        stateMachine.MoveTowards(currentPatrolTarget.position);
        if (Vector2.Distance(stateMachine.transform.position, currentPatrolTarget.position) < 0.5f)
        {
            currentPatrolTarget = (currentPatrolTarget == stateMachine.PointA) ? stateMachine.PointB : stateMachine.PointA;
        }
    }
}