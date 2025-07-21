using UnityEngine;

public class AttackDamageEvent : MonoBehaviour
{
    public EnemyData enemyData;
    public LayerMask playerLayer = default;

    // Chame este método via Animation Event
    public void DealAttackDamage(int attackIndex)
    {
        if (enemyData == null || enemyData.attacks == null || attackIndex >= enemyData.attacks.Length) return;

        var atk = enemyData.attacks[attackIndex];
        Vector3 offset = (Vector3)atk.offset;
        float rootScaleX = transform.root.lossyScale.x;
        if (rootScaleX < 0)
            offset.x = -offset.x;

        Vector3 origin = transform.position + offset;
        float range = atk.range;

        Collider2D hit = Physics2D.OverlapCircle(origin, range, playerLayer);
        if (hit != null && hit.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
        {
            playerHealth.TakeDamage(atk.damage);
        }
    }

    // Mantém o método antigo para compatibilidade
    public void DealAttackDamage()
    {
        DealAttackDamage(0);
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