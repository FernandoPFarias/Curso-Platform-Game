using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [Header("Configuração do Trampolim")]
    public Vector2 launchDirection = Vector2.up; // Direção padrão (para cima)
    public float launchForce = 15f; // Força do impulso
    public LayerMask playerLayer; // Layer do player para detecção

    [Header("Animação")]
    public Animator animator; // arraste o Animator do trampolim aqui no Inspector
    public string launchTrigger = "T_Launch"; // nome do parâmetro no Animator

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se o objeto é o player (pelo Layer)
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Zera a velocidade antes de lançar (opcional, para evitar bugs)
                rb.linearVelocity = Vector2.zero;
                // Aplica a força na direção configurada
                rb.AddForce(launchDirection.normalized * launchForce, ForceMode2D.Impulse);

                // Dispara a animação do trampolim
                if (animator != null)
                    animator.SetTrigger(launchTrigger);
            }
        }
    }

    // Gizmo para visualizar a direção no editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 dir = (Vector3)launchDirection.normalized * 2f;
        Gizmos.DrawLine(transform.position, transform.position + dir);
        Gizmos.DrawSphere(transform.position + dir, 0.1f);
    }
} 