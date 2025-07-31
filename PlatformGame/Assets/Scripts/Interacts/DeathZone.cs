using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public float penalty = 20f; // Vida perdida ao cair no abismo
    public GameObject playerPrefab; // Arraste o prefab do player aqui

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"DeathZone: Player entrou na DeathZone! Cena atual: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
            
            var health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                Debug.Log($"DeathZone: PlayerHealth encontrado! Vida atual: {health.CurrentHealth}, Penalidade: {penalty}");
                // Aplica o dano da DeathZone e respawna com penalidade
                health.RespawnFromDeathZone(penalty);
            }
            else
            {
                Debug.LogError("DeathZone: PlayerHealth não encontrado no player!");
            }
        }
    }
} 