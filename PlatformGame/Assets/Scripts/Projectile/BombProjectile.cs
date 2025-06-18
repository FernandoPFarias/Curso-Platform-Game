using UnityEngine;

public class BombProjectile : ProjectileBase
{
    public float speed = 10f;
    public float explosionRadius = 2f;
    public float lifetime = 3f;
    public float arcTimeToTarget = 0.8f; // Tempo para a bomba chegar ao alvo
    private float damage;
    private Rigidbody2D rb;
    private bool exploded = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime);
    }

    // Lança a bomba em arco para acertar o alvo em um tempo definido (configurado no Inspector)
    public void LaunchArc(Vector2 target)
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        Vector2 start = rb.position;
        Vector2 toTarget = target - start;
        float g = Mathf.Abs(Physics2D.gravity.y * rb.gravityScale);
        float t = arcTimeToTarget;
        float vx = toTarget.x / t;
        float vy = (toTarget.y + 0.5f * g * t * t) / t;
        rb.linearVelocity = new Vector2(vx, vy);
    }

    // Fallback: movimento reto
    public override void SetDirection(Vector2 direction)
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction.normalized * speed;
    }

    public override void SetDamage(float dmg)
    {
        damage = dmg;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!exploded)
        {
            Explode();
        }
    }

    private void Explode()
    {
        exploded = true;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player") && hit.TryGetComponent<PlayerHealth>(out var playerHealth))
            {
                playerHealth.TakeDamage(damage);
            }
        }
        // TODO: Adicionar efeitos visuais/sonoros de explosão aqui
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
} 