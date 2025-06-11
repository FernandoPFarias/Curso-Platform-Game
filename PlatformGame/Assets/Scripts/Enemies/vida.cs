using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health = 50f;

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        Debug.Log($"{gameObject.name} tomou {damageAmount} de dano! Vida restante: {health}");

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} foi derrotado!");
        // Desativa o objeto do inimigo. No futuro, você pode tocar uma animação de morte aqui.
        gameObject.SetActive(false);
    }
}