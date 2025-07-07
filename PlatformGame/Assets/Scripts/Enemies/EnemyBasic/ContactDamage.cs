using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    [Header("Data")]
    public EnemyData enemyData;

    private float contactCooldown = 1f;
    private float lastDamageTime;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.time >= lastDamageTime + contactCooldown)
            {
                if (other.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
                {
                    if (playerHealth.CurrentHealth <= enemyData.contactDamage)
                        playerHealth.HandlePlayerDeath(enemyData.contactDamage);
                    else
                        playerHealth.TakeDamage(enemyData.contactDamage);
                    lastDamageTime = Time.time;
                }
            }
        }
    }
}