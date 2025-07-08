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
                // Aplica o dano da DeathZone
                health.TakeDamage(penalty);
                
                // Se o player não morreu por dano (vida > 0), respawna sem perder vida extra
                if (health.CurrentHealth > 0)
                {
                    health.RespawnFromDeathZone();
                }
                // Se morreu por dano (vida <= 0), o método Die() já cuida da lógica de vidas extras
            }
        }
    }
} 