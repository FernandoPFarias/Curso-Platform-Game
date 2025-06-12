using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Data")]
    // Puxaremos o valor de vida m�xima da nossa "Ficha de Inimigo"
    public EnemyData enemyData;

    [Header("Event Channel to Raise on Death")]
    // Evento que ser� anunciado quando este inimigo morrer
    public GameEvent onDeathEvent;

    // --- Vari�veis de Estado ---
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
            Debug.LogError($"EnemyData n�o foi atribu�do no EnemyHealth do objeto {gameObject.name}!");
        }
    }

    // Este � o m�todo P�BLICO que outros scripts (como a hitbox do jogador) chamar�o.
    public void TakeDamage(float damageAmount)
    {
        // Se j� estiver morto, n�o faz mais nada.
        if (isDead) return;

        currentHealth -= damageAmount;
        Debug.Log($"{enemyData.enemyName} tomou {damageAmount} de dano. Vida atual: {currentHealth}");

        // L�gica de Morte
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

        // Exemplo simples: toca as part�culas de morte e destr�i o objeto.
        if (enemyData.deathParticlesPrefab != null)
        {
            Instantiate(enemyData.deathParticlesPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}