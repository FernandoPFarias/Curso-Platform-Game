using UnityEngine;

[CreateAssetMenu(menuName = "AI Behaviours/Sentry Behaviour")]
public class SentryBehaviour : AIBehaviour
{
    private enum State { Idling, Alert, Chasing }
    private State currentState;

    private float stateTimer;

    public override void Initialize(Enemy puppet)
    {
        currentState = State.Idling;
        if (puppet.Animator != null) puppet.Animator.SetTrigger("GoToIdle");
    }

    public override void Tick(Enemy puppet)
    {
        switch (currentState)
        {
            case State.Idling:
                // Fica parado, apenas virando na direção padrão se necessário
                puppet.FlipTowards(puppet.transform.position + (Vector3.right * puppet.transform.localScale.x));

                // Se detectar o jogador, muda para o estado de Alerta
                if (IsPlayerDetected(puppet))
                {
                    currentState = State.Alert;
                    stateTimer = puppet.EnemyData.alertDuration;
                    if (puppet.Animator != null) puppet.Animator.SetTrigger("GoToIdle"); // Ou um trigger "Alert"
                }
                break;

            case State.Alert:
                // A lógica de alerta é idêntica à do Patrulheiro
                stateTimer -= Time.fixedDeltaTime;
                if (stateTimer <= 0)
                {
                    currentState = State.Chasing;
                    if (puppet.Animator != null) puppet.Animator.SetTrigger("StartRun");
                }
                break;

            case State.Chasing:
                // A lógica de perseguição também pode ser reutilizada
                if (puppet.PlayerTarget == null || Vector2.Distance(puppet.transform.position, puppet.PlayerTarget.position) > puppet.EnemyData.giveUpRange)
                {
                    currentState = State.Idling; // Volta a ficar parado, não a patrulhar
                    if (puppet.Animator != null) puppet.Animator.SetTrigger("GoToIdle");
                    break;
                }
                puppet.MoveTowards(puppet.PlayerTarget.position);
                break;
        }
    }

    public override bool IsPlayerDetected(Enemy puppet)
    {
        if (puppet.PlayerTarget == null) return false;

        Vector2 directionToPlayer = puppet.PlayerTarget.position - puppet.transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer > puppet.EnemyData.detectionRange) return false;

        float facingDirection = Mathf.Sign(puppet.transform.localScale.x);
        float playerDirection = Mathf.Sign(directionToPlayer.x);

        // Só detecta se o jogador estiver na frente
        if (facingDirection != playerDirection) return false;

        // Dispara um Raycast para ver se há paredes no caminho
        RaycastHit2D hit = Physics2D.Raycast(puppet.transform.position, directionToPlayer.normalized, distanceToPlayer, LayerMask.GetMask("Player", "Ground"));

        // Se o que ele atingiu primeiro foi o jogador, então ele foi detectado!
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            Debug.Log("Sentinela: Jogador detectado com Raycast!");
            return true;
        }

        return false;
    }
}