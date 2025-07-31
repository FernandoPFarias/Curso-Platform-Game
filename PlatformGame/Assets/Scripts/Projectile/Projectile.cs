using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 15f;
    public float lifetime = 5f; // Tempo até o projétil se destruir sozinho

    private Rigidbody2D rb;
    private bool directionSet = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!directionSet)
        {
            // Encontra o jogador e atira na direção dele (fallback)
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Vector2 direction = (player.transform.position - transform.position).normalized;
                rb.linearVelocity = direction * speed;
            }
        }
        // Destrói o projétil depois de um tempo para não poluir a cena
        Destroy(gameObject, lifetime);
    }

    // Permite setar a direção de fora
    public void SetDirection(Vector2 direction)
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction.normalized * speed;
        directionSet = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
            {
                // Sempre usa TakeDamage - o PlayerHealth vai decidir se é morte real ou não
                playerHealth.TakeDamage(damage);
            }
            Destroy(gameObject); // Destrói ao atingir o jogador
        }
    }
}