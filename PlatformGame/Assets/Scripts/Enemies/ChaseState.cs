using UnityEngine;

public class ChaseState : AIState
{
    public ChaseState(Enemy enemy) : base(enemy) { }

    public override void OnEnter()
    {
        Debug.Log("Entrando no estado de Perseguição.");
        

    }

    public override void Tick()
    {
        // Se o jogador fugir, volta a patrulhar.
        if (Vector2.Distance(stateMachine.transform.position, stateMachine.PlayerTarget.position) > stateMachine.EnemyData.giveUpRange)
        {
            stateMachine.ChangeState(new PatrolState(stateMachine));
            return;
        }

        // Se for um inimigo Melee e estiver perto o suficiente, ele "ataca" ao colidir.
        // A lógica de dano por toque no OnCollisionEnter2D do Enemy.cs ainda funciona.
        if (stateMachine.EnemyData is MeleeEnemyData)
        {
            stateMachine.MoveTowards(stateMachine.PlayerTarget.position);
        }
        // Se for um inimigo Ranged e estiver na distância ideal, muda para o estado de Ataque.
        else if (stateMachine.EnemyData is RangedEnemyData rangedData)
        {
            if (Vector2.Distance(stateMachine.transform.position, stateMachine.PlayerTarget.position) < rangedData.visionRange)
            {
                //stateMachine.ChangeState(new RangedAttackState(stateMachine));
            }
            else // Se estiver fora de alcance, continua perseguindo
            {
                stateMachine.MoveTowards(stateMachine.PlayerTarget.position);
            }
        }
    }
}