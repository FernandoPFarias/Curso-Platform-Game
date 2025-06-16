using UnityEngine;

public class IdleState : AIState
{
    public IdleState(Enemy enemy) : base(enemy) { }

    public override void OnEnter()
    {
        Debug.Log("Inimigo entrou no estado Idle (parado).");
        // Garante que o inimigo esteja completamente parado ao entrar neste estado.
        stateMachine.Rb.linearVelocity = Vector2.zero;

        // Dispara a animação de 'Idle'
        stateMachine.Animator.SetTrigger("GoToIdle");
    }

    public override void Tick()
    {
        // A única lógica deste estado é verificar se o jogador se aproximou.
        if (stateMachine.PlayerTarget != null &&
            Vector2.Distance(stateMachine.transform.position, stateMachine.PlayerTarget.position) < stateMachine.EnemyData.detectionRange)
        {
            // ...muda para o estado de Alerta.
            stateMachine.ChangeState(new AlertState(stateMachine));
        }
    }
}