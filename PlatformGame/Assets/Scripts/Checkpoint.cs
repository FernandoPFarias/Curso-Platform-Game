using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public float respawnYOffset = 0.3f; // Ajustável no Inspector

    private Animator animator;
    private bool isActive = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        // Garante que começa no estado Idle
        if (animator != null)
            animator.SetBool("B_IsActive", false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isActive)
        {
            // Desativa todos os outros checkpoints da cena
            foreach (var cp in FindObjectsOfType<Checkpoint>())
            {
                if (cp != this)
                    cp.Deactivate();
            }

            GameManager.Instance.lastCheckpointPosition = transform.position;
            GameManager.Instance.lastCheckpointYOffset = respawnYOffset; // Salva o offset junto
            isActive = true;
            if (animator != null)
                animator.SetBool("B_IsActive", true);
            // Aqui você pode adicionar efeitos visuais ou som de checkpoint ativado
            Debug.Log("Checkpoint ativado em: " + transform.position);
        }
    }

    public void Deactivate()
    {
        isActive = false;
        if (animator != null)
            animator.SetBool("B_IsActive", false);
    }

    // Gizmo para visualizar o respawn
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Vector3 respawnPos = transform.position + Vector3.up * respawnYOffset;
        Gizmos.DrawWireSphere(respawnPos, 0.2f);
        Gizmos.DrawLine(transform.position, respawnPos);
    }
} 