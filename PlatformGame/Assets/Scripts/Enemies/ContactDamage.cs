using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    [Header("Data")]
    // Conecte aqui o mesmo asset de EnemyData do inimigo
    public EnemyData enemyData;

    // Cooldown para evitar que o dano seja aplicado em todos os frames de contato
    private float contactCooldown = 1f;
    private float lastDamageTime;

   
    private void OnTriggerEnter2D(Collider2D other)
    {
       
        if (other.CompareTag("Player"))
        {
            
            if (Time.time >= lastDamageTime + contactCooldown)
            {
                // Tenta encontrar o componente de vida do jogador no objeto que entrou no trigger
                if (other.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
                {
                    Debug.Log($"Inimigo {gameObject.name} causou {enemyData.contactDamage} de dano por contato.");

                    // Aplica o dano de CONTATO, lido diretamente da ficha do inimigo
                    playerHealth.TakeDamage(enemyData.contactDamage);

                    // Atualiza o relógio para o próximo cooldown
                    lastDamageTime = Time.time;
                }
            }
        }
    }
}