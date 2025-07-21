using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Data")]
    // Puxaremos o valor de vida máxima da nossa "Ficha de Inimigo"
    public EnemyData enemyData;

    [Header("Event Channel to Raise on Death")]
    // Evento que será anunciado quando este inimigo morrer
    public GameEvent onDeathEvent;

    private Enemy enemy;
    private EnemyAnimator enemyAnimator;
    // --- Variáveis de Estado ---
    private float currentHealth;
    private bool isDead = false;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        if (enemy != null)
        {
            enemyAnimator = enemy.animationManager;
            if (enemy.enemyData != null)
                currentHealth = enemy.enemyData.maxHealth;
            else
                Debug.LogError($"EnemyData não foi atribuído no Enemy do objeto {gameObject.name}!");
        }
        else
        {
            Debug.LogError($"Enemy não encontrado no objeto {gameObject.name}!");
        }
    }

    // Este é o método PÚBLICO que a hitbox do jogador vai chamar.
    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;
        Debug.Log($"{enemy.enemyData.enemyName} tomou {damageAmount} de dano! Vida atual: {currentHealth}");

        // Checagem de Morte é a PRIORIDADE MÁXIMA
        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        // Só toca animação de hurt se não morreu
        if (enemyAnimator != null)
        {
            enemyAnimator.TriggerHurt();
            Debug.Log($"Animação de hurt disparada para {enemy.enemyData.enemyName}");
        }
        else
        {
            Debug.LogWarning($"EnemyAnimator não encontrado em {gameObject.name}!");
        }

        // Modular: chama o comportamento do inimigo via Enemy
        if (enemy != null && enemy.GetBehaviour() != null)
        {
            enemy.GetBehaviour().OnTakeDamage(enemy);
            Debug.Log($"Estado de knockback ativado para {enemy.enemyData.enemyName}");
        }
        else
        {
            Debug.LogWarning($"Behaviour não encontrado em {gameObject.name}!");
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log($"{enemy.enemyData.enemyName} foi derrotado!");

        // 1. Dispara a animação de morte
        if (enemyAnimator != null)
        {
            enemyAnimator.TriggerDeath();
            Debug.Log($"Animação de death disparada para {enemy.enemyData.enemyName}");
        }
        else
        {
            Debug.LogWarning($"EnemyAnimator não encontrado em {gameObject.name}!");
        }

        // 2. Anuncia para o resto do jogo que o inimigo morreu
        onDeathEvent?.Raise();

        // 3. Desliga a "inteligência" do inimigo
        if (enemy != null)
        {
            // Para todas as corrotinas do Enemy
            enemy.StopAllCoroutines();
            // Para corrotinas do BossBehaviour, se for boss
            var bossBehaviour = enemy.GetBehaviour() as BossBehaviour;
            if (bossBehaviour != null)
            {
                // Não há referência direta à corrotina, mas desabilitar o Enemy já impede novas execuções
            }
            enemy.enabled = false;
        }

        // 4. Congela o Rigidbody2D completamente
        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.gravityScale = 0f; // Garante que não caia
        }

        // 5. Desativa o Collider2D (opcional, pode deixar ativado se quiser corpo visível)
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        // 6. Desativa scripts de ataque e hitboxes filhos
        foreach (var atk in GetComponentsInChildren<AttackDamageEvent>())
            atk.enabled = false;
        foreach (var contact in GetComponentsInChildren<ContactDamage>())
            contact.enabled = false;
        foreach (var collider in GetComponentsInChildren<Collider2D>())
        {
            if (collider != col) collider.enabled = false;
        }

        // 7. Agenda a destruição do objeto, dando tempo para a animação tocar.
        Destroy(gameObject, 3f); // Ajuste o tempo conforme sua animação de morte.
    }
}