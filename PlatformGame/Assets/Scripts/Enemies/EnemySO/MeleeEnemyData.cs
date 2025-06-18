using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Data/Melee Enemy")]
public class MeleeEnemyData : EnemyData
{
    [Header("Melee Attack Stats")]
    public float attackDamage = 10f;
    public float attackCooldown = 1f;
    public string attackType = "Melee";
    // Adicione aqui outros campos exclusivos do melee, como efeitos, animações específicas, etc.
}