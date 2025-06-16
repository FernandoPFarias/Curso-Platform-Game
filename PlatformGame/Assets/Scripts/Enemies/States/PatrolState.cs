using UnityEngine;

public class PatrolState : AIState
{
    private Transform currentPatrolTarget;

    public PatrolState(Enemy enemy) : base(enemy) { }

    public override void OnEnter()
    {
        stateMachine.Animator.SetFloat("T_StartPatrol", 1f);
        currentPatrolTarget = stateMachine.PointA;
    }

    public override void Tick()
    {
        if (stateMachine.PlayerTarget != null && stateMachine.EnemyData.behaviour.IsPlayerDetected(stateMachine))
        {
            stateMachine.ChangeState(new AlertState(stateMachine));
            return;
        }

        stateMachine.MoveTowards(currentPatrolTarget.position);
        stateMachine.FlipTowards(currentPatrolTarget.position);

        if (Vector2.Distance(stateMachine.transform.position, currentPatrolTarget.position) < 0.5f)
        {
            currentPatrolTarget = (currentPatrolTarget == stateMachine.PointA) ? stateMachine.PointB : stateMachine.PointA;
        }
    }
}