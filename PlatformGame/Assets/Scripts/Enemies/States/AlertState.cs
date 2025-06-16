using UnityEngine;

public class AlertState : AIState
{
    private float alertTimer;

    public AlertState(Enemy enemy) : base(enemy) { }

    public override void OnEnter()
    {
        stateMachine.Rb.linearVelocity = Vector2.zero;
        stateMachine.Animator.SetBool("B_isAlerting", true); // Liga o interruptor de prioridade
        stateMachine.Animator.SetTrigger("T_GoToIdle"); // ou TriggerAlert se tiver a animação

        if (stateMachine.PlayerTarget != null)
        {
            stateMachine.FlipTowards(stateMachine.PlayerTarget.position);
        }

        alertTimer = stateMachine.EnemyData.alertDuration;
    }

    public override void Tick()
    {
        alertTimer -= Time.fixedDeltaTime;
        if (alertTimer <= 0)
        {
            stateMachine.ChangeState(new ChaseState(stateMachine));
        }
    }

    public override void OnExit()
    {
        stateMachine.Animator.SetBool("B_isAlerting", false); // Desliga o interruptor
    }
}