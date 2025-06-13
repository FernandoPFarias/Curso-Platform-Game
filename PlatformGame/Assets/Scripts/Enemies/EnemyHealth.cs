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

    private Enemy stateMachine;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        stateMachine = GetComponent<Enemy>();

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
            return;
        }
       
        
            // Em vez de s� tocar a anima��o, manda a IA inteira para o estado de "Hurt".
            stateMachine.ChangeState(new KnockbackState(stateMachine));
        


    }

    private void Die()
    {
        isDead = true;
        Debug.Log($"{enemyData.enemyName} foi derrotado!");


        Debug.Log("<color=orange>Tentando disparar o gatilho Death...</color>");
        animator?.SetTrigger("Death");
        Debug.Log("<color=lime>Gatilho Death disparado.</color>");
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