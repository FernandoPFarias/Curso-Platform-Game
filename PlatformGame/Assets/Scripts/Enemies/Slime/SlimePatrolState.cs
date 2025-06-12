using UnityEngine;

public class SlimePatrolState : AIState
{
    private float pauseTimer;
    private SlimeData slimeData; // Para acessar os dados espec�ficos do Slime

    public SlimePatrolState(Enemy enemy) : base(enemy)
    {
        // Converte os dados gen�ricos para os dados espec�ficos do Slime
        slimeData = enemy.EnemyData as SlimeData;
    }

    public override void OnEnter()
    {
        Debug.Log("Slime: Entrando no estado de Pulo/Patrulha.");
        // Ao entrar no estado, ele come�a a pausa.
        pauseTimer = slimeData.hopPauseDuration;
    }

    public override void Tick()
    {
        // A prioridade m�xima � sempre checar se o jogador est� perto.
        if (Vector2.Distance(stateMachine.transform.position, stateMachine.PlayerTarget.position) < slimeData.detectionRange)
        {
            stateMachine.ChangeState(new ChaseState(stateMachine));
            return;
        }

        // L�gica da Pausa
        pauseTimer -= Time.deltaTime;
        if (pauseTimer <= 0)
        {
            // O tempo de pausa acabou. Hora de pular!
            Hop();
            // Reseta o timer para a pr�xima pausa.
            pauseTimer = slimeData.hopPauseDuration;
        }
    }

    private void Hop()
    {
        Debug.Log("Slime: Pulando!");
        // Vira para a dire��o do alvo de patrulha antes de pular
       // var target = stateMachine.GetCurrentPatrolTarget();
       // stateMachine.FlipTowards(target.position);

        // Aplica uma for�a para cima e para frente
        float direction = stateMachine.transform.localScale.x;
        Vector2 force = new Vector2(direction * (slimeData.hopForce / 2), slimeData.hopForce);

        stateMachine.Rb.linearVelocity = Vector2.zero; // Zera a velocidade para um pulo consistente
        stateMachine.Rb.AddForce(force, ForceMode2D.Impulse);
    }
}
