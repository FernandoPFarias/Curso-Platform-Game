using UnityEngine;

public class AttackState : AIState
{
    private float attackCooldownTimer;

    // ESTA É A SINTAXE CORRETA DO CONSTRUTOR
    public AttackState(Enemy enemy) : base(enemy) { }

    public override void OnEnter()
    {
        if (stateMachine.EnemyData is MeleeEnemyData meleeData)
        {
            // NOVA VERIFICAÇÃO:
            // Só dispara o gatilho se um nome foi fornecido na Ficha do Inimigo.
            if (!string.IsNullOrEmpty(meleeData.attackTriggerName))
            {
                stateMachine.Animator.SetTrigger(meleeData.attackTriggerName);
            }

            attackCooldownTimer = meleeData.attackCooldown;
        }
    }

    public override void Tick()
    {
        stateMachine.FlipTowards(stateMachine.PlayerTarget.position);

        attackCooldownTimer -= Time.deltaTime;
        if (attackCooldownTimer <= 0)
        {
            stateMachine.ChangeState(new ChaseState(stateMachine));
        }
    }

    public override void OnExit()
    {
        Debug.Log("Inimigo saiu do estado de Ataque.");
    }
}