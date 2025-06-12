using UnityEngine;

public class SlimePatrolState : AIState
{
    private float pauseTimer;
    private SlimeData slimeData; // Para acessar os dados específicos do Slime

    public SlimePatrolState(Enemy enemy) : base(enemy)
    {
        // Converte os dados genéricos para os dados específicos do Slime
        slimeData = enemy.EnemyData as SlimeData;
    }

    public override void OnEnter()
    {
        Debug.Log("Slime: Entrando no estado de Pulo/Patrulha.");
        // Ao entrar no estado, ele começa a pausa.
        pauseTimer = slimeData.hopPauseDuration;
    }

    public override void Tick()
    {
        // A prioridade máxima é sempre checar se o jogador está perto.
        if (Vector2.Distance(stateMachine.transform.position, stateMachine.PlayerTarget.position) < slimeData.detectionRange)
        {
            stateMachine.ChangeState(new ChaseState(stateMachine));
            return;
        }

        // Lógica da Pausa
        pauseTimer -= Time.deltaTime;
        if (pauseTimer <= 0)
        {
            // O tempo de pausa acabou. Hora de pular!
            Hop();
            // Reseta o timer para a próxima pausa.
            pauseTimer = slimeData.hopPauseDuration;
        }
    }

    private void Hop()
    {
        Debug.Log("Slime: Pulando!");
        // Vira para a direção do alvo de patrulha antes de pular
       // var target = stateMachine.GetCurrentPatrolTarget();
       // stateMachine.FlipTowards(target.position);

        // Aplica uma força para cima e para frente
        float direction = stateMachine.transform.localScale.x;
        Vector2 force = new Vector2(direction * (slimeData.hopForce / 2), slimeData.hopForce);

        stateMachine.Rb.linearVelocity = Vector2.zero; // Zera a velocidade para um pulo consistente
        stateMachine.Rb.AddForce(force, ForceMode2D.Impulse);
    }
}
