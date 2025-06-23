using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    private bool isDead = false;

    private Animator animator;

    private void Start()
    {
        currentHealth = maxHealth;

        animator = GetComponentInChildren<Animator>();

    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;
        Debug.Log($"JOGADOR tomou {damageAmount} de dano! Vida atual: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        animator?.SetTrigger("Hurt");

    }

    private void Die()
    {   
        if (isDead) return;
        isDead = true;

        animator?.SetTrigger("Death");


        // Desliga os scripts de controle para que o jogador n�o possa mais se mover ou atacar
        GetComponent<PlayerController>().enabled = false;
        GetComponent<PlayerCombat>().enabled = false;

        // Desliga a f�sica para o corpo n�o cair ou ser empurrado
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;

        // Desliga o collider para n�o interagir mais com nada
        GetComponent<Collider2D>().enabled = false;

        // Aqui voc� chamaria seu GameManager para mostrar a tela de "Game Over"
        // Ex: GameManager.Instance.ShowGameOverScreen();


        Debug.Log("Jogador foi derrotado!");
        
    }

    public void Heal(int amount)
    {
        if (isDead) return;
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
        Debug.Log($"JOGADOR curado em {amount}! Vida atual: {currentHealth}");
    }
}