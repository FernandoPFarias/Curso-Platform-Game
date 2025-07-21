using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public float penalty = 20f; // Vida perdida ao cair no abismo
    public GameObject playerPrefab; // Arraste o prefab do player aqui

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                // Aplica o dano da DeathZone e respawna com penalidade
                health.RespawnFromDeathZone(penalty);
            }
        }
    }
} 