using UnityEngine;
using System.Collections.Generic;

public class BombProjectile : ProjectileBase
{
    public float speed = 10f;
    public float explosionRadius = 2f;
    public float lifetime = 3f;
    public float arcTimeToTarget = 0.8f; // Tempo para a bomba chegar ao alvo
    private float damage;
    private Rigidbody2D rb;
    private bool exploded = false;
    [SerializeField] private Animator animator; // Arraste o Animator correto aqui pelo Inspector
    private bool isExploding = false;
    [SerializeField] private ProjectileAnimatorHandler animHandler; // Arraste o handler do filho visual aqui
    [SerializeField] private LayerMask explodeOnLayers; // Configure as layers que detonam a bomba via Inspector
    [SerializeField] private List<string> explodeOnTags = new List<string>(); // Adicione as tags que detonam a bomba

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animHandler?.PlayLaunch();
        if (animator != null)
            animator.SetTrigger("T_Bomb");
        //Destroy(gameObject);
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (exploded) return;
        bool layerMatch = ((1 << other.gameObject.layer) & explodeOnLayers) != 0;
        bool tagMatch = explodeOnTags.Contains(other.tag);
        if (layerMatch || tagMatch)
        {
            // Para o movimento imediatamente e trava a gravidade
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.gravityScale = 0f;
            }
            Explode();
        }
    }

    private void Explode()
    {
        if (isExploding) return;
        isExploding = true;
        exploded = true;

        // Desativa o collider para evitar múltiplas explosões
        var col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player") && hit.TryGetComponent<PlayerHealth>(out var playerHealth))
            {
                if (playerHealth.CurrentHealth <= damage)
                    playerHealth.HandlePlayerDeath(damage);
                else
                    playerHealth.TakeDamage(damage);
            }
        }
        animHandler?.PlayExplosion();
        // NÃO destrua aqui! O Animation Event do handler cuidará disso.
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
} 