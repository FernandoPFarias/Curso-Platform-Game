using UnityEngine;

public class HurtState : AIState
{
    private float hurtTimer;

    public HurtState(Enemy enemy) : base(enemy) { }

    public override void OnEnter()
    {
        Debug.Log("Inimigo entrou no estado de 'Hurt' (Tomou Dano).");

        // Dispara a anima��o de tomar dano
        stateMachine.Animator.SetTrigger("Hurt");

        // Para completamente o movimento do inimigo.
        stateMachine.Rb.linearVelocity = Vector2.zero;

        // Inicia o timer de paralisia com o valor da nossa "Ficha de Inimigo".
        hurtTimer = stateMachine.EnemyData.hurtDuration;
    }

    public override void Tick()
    {
        // Decrementa o timer a cada ciclo de f�sica.
        hurtTimer -= Time.fixedDeltaTime;

        // CONDI��O DE TRANSI��O: O tempo de paralisia acabou?
        if (hurtTimer <= 0)
        {
            // Se sim, volta a perseguir o jogador (uma resposta agressiva e comum).
            stateMachine.ChangeState(new ChaseState(stateMachine));
        }
    }

    public override void OnExit()
    {
        // Nada a fazer ao sair, o pr�ximo estado cuidar� da anima��o e do movimento.
    }
}