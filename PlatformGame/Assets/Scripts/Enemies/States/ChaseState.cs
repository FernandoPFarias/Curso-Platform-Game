using UnityEngine;

public class ChaseState : AIState
{
    public ChaseState(Enemy enemy) : base(enemy) { }

    public override void OnEnter()
    {
        stateMachine.Animator.SetFloat("T_StartPatrol", 1f);
    }

    public override void Tick()
    {
        if (stateMachine.PlayerTarget == null)
        {
            stateMachine.ChangeState(new PatrolState(stateMachine));
            return;
        }

        if (Vector2.Distance(stateMachine.transform.position, stateMachine.PlayerTarget.position) > stateMachine.EnemyData.giveUpRange)
        {
            stateMachine.ChangeState(new PatrolState(stateMachine));
            return;
        }

        if (stateMachine.EnemyData is MeleeEnemyData meleeData)
        {
            if (Vector2.Distance(stateMachine.transform.position, stateMachine.PlayerTarget.position) < meleeData.attackRadius)
            {
                stateMachine.ChangeState(new AttackState(stateMachine));
                return;
            }
        }

        stateMachine.MoveTowards(stateMachine.PlayerTarget.position);
    }
}