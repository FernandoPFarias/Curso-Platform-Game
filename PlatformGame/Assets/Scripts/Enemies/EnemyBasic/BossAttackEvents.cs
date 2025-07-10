using UnityEngine;

public class BossAttackEvents : MonoBehaviour
{
    [Header("Configuração do Ataque")]
    public GameObject upAttackHitboxPrefab;
    public GameObject downAttackHitboxPrefab;
    public Transform attackSpawnPoint;
    public float hitboxDuration = 0.3f;

    // Chame este método via Animation Event na animação de baixo para cima
    public void BossAttack_Up()
    {
        Debug.Log("Ataque de baixo para cima!");
        if (upAttackHitboxPrefab != null && attackSpawnPoint != null)
        {
            var hitbox = Instantiate(upAttackHitboxPrefab, attackSpawnPoint.position, attackSpawnPoint.rotation);
            Destroy(hitbox, hitboxDuration);
        }
    }

    // Chame este método via Animation Event na animação de cima para baixo
    public void BossAttack_Down()
    {
        Debug.Log("Ataque de cima para baixo!");
        if (downAttackHitboxPrefab != null && attackSpawnPoint != null)
        {
            var hitbox = Instantiate(downAttackHitboxPrefab, attackSpawnPoint.position, attackSpawnPoint.rotation);
            Destroy(hitbox, hitboxDuration);
        }
    }
} 