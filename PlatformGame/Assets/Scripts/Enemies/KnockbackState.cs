using UnityEngine;

public class KnockbackState : AIState
{
    private float knockbackTimer;

    public KnockbackState(Enemy enemy): base(enemy) { }


    public override void OnEnter()
    {
        knockbackTimer = stateMachine.EnemyData.knockbackDuration;

        stateMachine.Animator.SetTrigger("Hurt");


        stateMachine.Rb.linearVelocity = Vector2.zero;

        Vector2 direction =(stateMachine.transform.position - stateMachine.PlayerTarget.position).normalized;

        Vector2 force = new Vector2(direction.x * stateMachine.EnemyData.knockbackForce.x, stateMachine.EnemyData.knockbackForce.y);
        
        // DEBUG: Mostra no console qual força estamos tentando aplicar.
        Debug.Log($"<color=yellow>KNOCKBACK: Aplicando força de {force} ao inimigo!</color>");


        stateMachine.Rb.AddForce(force, ForceMode2D.Impulse);


    }


    public override void Tick()
    {
        knockbackTimer -= Time.deltaTime;

        if (knockbackTimer <= 0)
        {

            stateMachine.ChangeState(new ChaseState(stateMachine));
        }
    }



}
