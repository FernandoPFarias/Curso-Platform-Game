using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Data")]
    // Puxaremos o valor de vida máxima da nossa "Ficha de Inimigo"
    public EnemyData enemyData;

    [Header("Event Channel to Raise on Death")]
    // Evento que será anunciado quando este inimigo morrer
    public GameEvent onDeathEvent;

    private Animator animator;

    // --- Variáveis de Estado ---
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
            return;
        }
       
        
            // Em vez de só tocar a animação, manda a IA inteira para o estado de "Hurt".
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