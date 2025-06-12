using UnityEngine;

public class AlertState : AIState
{
    private float alertTimer;

    public AlertState(Enemy enemy) : base(enemy) { }

    public override void OnEnter()
    {
        // 1. PARA o movimento do inimigo.
        stateMachine.Rb.linearVelocity = Vector2.zero;

        // 2. VIRA imediatamente para encarar o jogador.
        stateMachine.FlipTowards(stateMachine.PlayerTarget.position);

        stateMachine.Animator.SetBool("isActing", true);
        stateMachine.Animator.SetTrigger("TriggerAlert");

        // 3. Inicia o timer de PAUSA.
        alertTimer = stateMachine.EnemyData.alertDuration;


        

    }

    public override void Tick()
    {
        alertTimer -= Time.deltaTime;

        // Se o tempo de alerta acabou, muda para o estado de Perseguição.
        if (alertTimer <= 0)
        {
            stateMachine.ChangeState(new ChaseState(stateMachine));
        }
        // Se o jogador fugir durante a pausa, volta a patrulhar.
        else if (Vector2.Distance(stateMachine.transform.position, stateMachine.PlayerTarget.position) > stateMachine.EnemyData.giveUpRange)
        {
            stateMachine.ChangeState(new PatrolState(stateMachine));
        }
    }
    // OnExit é chamado QUANDO O ESTADO ESTÁ ACABANDO
    public override void OnExit()
    {
        // DESLIGA O INTERRUPTOR, liberando as animações de locomoção
        stateMachine.Animator.SetBool("isActing", false);
    }
}