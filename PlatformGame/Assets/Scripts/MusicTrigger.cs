using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    [Header("Music Settings")]
    public AudioClip areaMusic;
    public float fadeTime = 2f;
    public bool returnToBackgroundOnExit = true;

    [Header("Trigger Settings")]
    public bool triggerOnEnter = true;
    public bool triggerOnExit = true;

    private bool playerInArea = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && triggerOnEnter)
        {
            playerInArea = true;
            if (AudioManager.Instance != null && areaMusic != null)
            {
                AudioManager.Instance.CrossfadeToMusic(areaMusic, fadeTime);
                Debug.Log($"[MusicTrigger] Entrou na área: {gameObject.name}, música: {areaMusic.name}");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && triggerOnExit && returnToBackgroundOnExit)
        {
            playerInArea = false;
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.ReturnToBackgroundMusic(fadeTime);
                Debug.Log($"[MusicTrigger] Saiu da área: {gameObject.name}, voltando para música de fundo");
            }
        }
    }

    // Método público para forçar a transição (útil para scripts externos)
    public void TriggerMusic()
    {
        if (AudioManager.Instance != null && areaMusic != null)
        {
            AudioManager.Instance.CrossfadeToMusic(areaMusic, fadeTime);
        }
    }

    // Método público para voltar à música de fundo
    public void ReturnToBackground()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ReturnToBackgroundMusic(fadeTime);
        }
    }

    private void OnDrawGizmos()
    {
        // Desenha o trigger no editor
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            Gizmos.color = Color.green;
            if (col is BoxCollider2D boxCol)
            {
                Vector3 size = new Vector3(boxCol.size.x, boxCol.size.y, 0.1f);
                Gizmos.DrawWireCube(transform.position, size);
            }
            else if (col is CircleCollider2D circleCol)
            {
                Gizmos.DrawWireSphere(transform.position, circleCol.radius);
            }
        }
    }
} 