using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 15f;
    public float lifetime = 5f; // Tempo at� o proj�til se destruir sozinho

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Encontra o jogador e atira na dire��o dele
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            rb.linearVelocity = direction * speed;
        }
        // Destr�i o proj�til depois de um tempo para n�o poluir a cena
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
            {
                playerHealth.TakeDamage(damage);
            }
            Destroy(gameObject); // Destr�i ao atingir o jogador
        }
    }
}