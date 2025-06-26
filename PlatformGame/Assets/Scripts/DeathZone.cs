using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public float penalty = 20f; // Vida perdida ao cair no abismo
    public GameObject playerPrefab; // Arraste o prefab do player aqui

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Salva a vida com penalidade
            var health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                float newHealth = Mathf.Max(1f, health.CurrentHealth - penalty);
                if (GameManager.Instance != null)
                    GameManager.Instance.playerHealth = newHealth;
            }

            // Salva a posição do checkpoint
            Vector3 spawnPos = GameManager.Instance != null
                ? GameManager.Instance.lastCheckpointPosition + Vector3.up * GameManager.Instance.lastCheckpointYOffset
                : other.transform.position;

            // Destroi o player antigo
            Destroy(other.gameObject);

            // Instancia um novo player no checkpoint
            GameObject newPlayer = Instantiate(playerPrefab, spawnPos, Quaternion.identity);

            // Garante velocidade zerada
            var rb = newPlayer.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            // Inicia invencibilidade/piscada
            var newPlayerHealth = newPlayer.GetComponent<PlayerHealth>();
            if (newPlayerHealth != null)
            {
                newPlayerHealth.StartInvincibility();
            }
        }
    }
} 