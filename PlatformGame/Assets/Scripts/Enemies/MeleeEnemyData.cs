using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "EnemyData/Melee Enemy")]
public class MeleeEnemyData : EnemyData
{

    [Header("Melee Attack Status")]
    public float attackDamage;
    public float attackRadius;
    public float attackCooldown;

}
