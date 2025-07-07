using UnityEngine;

public class AttackDamageEvent : MonoBehaviour
{
    public EnemyData enemyData;
    public LayerMask playerLayer = default;

    // Chame este método via Animation Event
    public void DealAttackDamage()
    {
        if (enemyData == null) return;

        // Calcula a posição do ataque baseada no offset do SO e na direção do inimigo
        Vector3 offset = (Vector3)enemyData.attackOffset;
        float rootScaleX = transform.root.lossyScale.x;
        if (rootScaleX < 0)
            offset.x = -offset.x;

        Vector3 origin = transform.position + offset;
        float range = enemyData.attackRange;

        Collider2D hit = Physics2D.OverlapCircle(origin, range, playerLayer);
        if (hit != null && hit.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
        {
            if (playerHealth.CurrentHealth <= enemyData.contactDamage)
                playerHealth.HandlePlayerDeath(enemyData.contactDamage);
            else
                playerHealth.TakeDamage(enemyData.contactDamage);
        }
    }

    // Gizmo para visualizar o alcance no editor
    private void OnDrawGizmosSelected()
    {
        if (enemyData == null) return;

        Vector3 offset = (Vector3)enemyData.attackOffset;
        float rootScaleX = transform.root.lossyScale.x;
        if (rootScaleX < 0)
            offset.x = -offset.x;

        Vector3 origin = transform.position + offset;
        float range = enemyData.attackRange;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(origin, range);
    }
} 