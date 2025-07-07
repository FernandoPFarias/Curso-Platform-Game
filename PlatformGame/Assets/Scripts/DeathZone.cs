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
                bool morreu = health.CurrentHealth <= penalty;
                health.TakeDamage(penalty);
                if (!morreu)
                {
                    // Força o respawn manualmente (com fade)
                    health.HandlePlayerDeath(0); // penalidade zero, pois já foi aplicada
                }
            }
        }
    }
} 