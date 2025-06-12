using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Data")]
    // Puxaremos o valor de vida máxima da nossa "Ficha de Inimigo"
    public EnemyData enemyData;

    [Header("Event Channel to Raise on Death")]
    // Evento que será anunciado quando este inimigo morrer
    public GameEvent onDeathEvent;

    // --- Variáveis de Estado ---
    private float currentHealth;
    private bool isDead = false;

    private void Start()
    {
        if (enemyData != null)
        {
            // Inicializa a vida com o valor da nossa ficha de ScriptableObject
            currentHealth = enemyData.maxHealth;
        }
        else
        {
            Debug.LogError($"EnemyData não foi atribuído no EnemyHealth do objeto {gameObject.name}!");
        }
    }

    // Este é o método PÚBLICO que outros scripts (como a hitbox do jogador) chamarão.
    public void TakeDamage(float damageAmount)
    {
        // Se já estiver morto, não faz mais nada.
        if (isDead) return;

        currentHealth -= damageAmount;
        Debug.Log($"{enemyData.enemyName} tomou {damageAmount} de dano. Vida atual: {currentHealth}");

        // Lógica de Morte
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log($"{enemyData.enemyName} foi derrotado!");

        // Anuncia para todo o jogo que este inimigo morreu.
        // Um ScoreManager ou um LootManager poderiam ouvir este evento.
        onDeathEvent?.Raise();

        // Exemplo simples: toca as partículas de morte e destrói o objeto.
        if (enemyData.deathParticlesPrefab != null)
        {
            Instantiate(enemyData.deathParticlesPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}