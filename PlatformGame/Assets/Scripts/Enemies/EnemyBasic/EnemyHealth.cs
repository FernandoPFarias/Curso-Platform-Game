using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Data")]
    // Puxaremos o valor de vida máxima da nossa "Ficha de Inimigo"
    public EnemyData enemyData;

    [Header("Event Channel to Raise on Death")]
    // Evento que será anunciado quando este inimigo morrer
    public GameEvent onDeathEvent;

    // --- Componentes ---
    private Animator animator;
    private Enemy stateMachine; // Referência para o "cérebro" da IA para dar ordens

    // --- Variáveis de Estado ---
    private float currentHealth;
    private bool isDead = false;

    private void Start()
    {
        // Pega as referências necessárias que estão no mesmo GameObject ou em filhos
        animator = GetComponentInChildren<Animator>();
        stateMachine = GetComponent<Enemy>();

        if (enemyData != null)
        {
            currentHealth = enemyData.maxHealth;
        }
        else
        {
            Debug.LogError($"EnemyData não foi atribuído no EnemyHealth do objeto {gameObject.name}!");
        }
    }

    // Este é o método PÚBLICO que a hitbox do jogador vai chamar.
    public void TakeDamage(float damageAmount)
    {
        // Se já está morto, não faz mais nada para evitar bugs.
        if (isDead) return;

        currentHealth -= damageAmount;
        Debug.Log($"{enemyData.enemyName} tomou {damageAmount} de dano! Vida atual: {currentHealth}");

        // Checagem de Morte é a PRIORIDADE MÁXIMA
        if (currentHealth <= 0)
        {
            Die();
            return; // 'return' é crucial para garantir que nada mais seja executado.
        }

        // APENAS SE NÃO MORREU, ele comanda a IA para entrar no estado de knockback/dor.
        stateMachine.ChangeState(new KnockbackState(stateMachine));
    }

    private void Die()
    {
        isDead = true;
        Debug.Log($"{enemyData.enemyName} foi derrotado!");

        // 1. Dispara a animação de morte
        animator?.SetTrigger("T_Death");

        // 2. Anuncia para o resto do jogo que o inimigo morreu
        onDeathEvent?.Raise();

        // 3. Desliga a "inteligência" e a "física" do inimigo
        if (GetComponent<Enemy>() != null) GetComponent<Enemy>().enabled = false;
        if (GetComponent<Collider2D>() != null) GetComponent<Collider2D>().enabled = false;

        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = Vector2.zero;
        }

        // 4. Agenda a destruição do objeto, dando tempo para a animação tocar.
        Destroy(gameObject, 3f); // Ajuste o tempo conforme sua animação de morte.
    }
}