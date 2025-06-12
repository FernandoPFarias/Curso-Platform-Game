using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Data")]
    // Puxaremos o valor de vida m�xima da nossa "Ficha de Inimigo"
    public EnemyData enemyData;

    [Header("Event Channel to Raise on Death")]
    // Evento que ser� anunciado quando este inimigo morrer
    public GameEvent onDeathEvent;

    private Animator animator;

    // --- Vari�veis de Estado ---
    private float currentHealth;
    private bool isDead = false;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();

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
        else
        {
            animator?.SetTrigger("Hurt"); ;
        }


    }

    private void Die()
    {
        isDead = true;
        Debug.Log($"{enemyData.enemyName} foi derrotado!");


        animator?.SetTrigger("Death");
        // Anuncia para todo o jogo que este inimigo morreu.
        // Um ScoreManager ou um LootManager poderiam ouvir este evento.
        onDeathEvent?.Raise();


        GetComponent<Enemy>().enabled = false;

        this.enabled = false;

        GetComponent<Collider2D>().enabled = false;

        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;



        Destroy(gameObject, 2f);
    }
}