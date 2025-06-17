using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Data/Melee Enemy")]
public class MeleeEnemyData : EnemyData
{
    [Header("Melee Attack Stats")]
    public float attackDamage;
    public float attackRadius;
    public float attackCooldown;
    public string attackTriggerName; // Nome do gatilho de ataque no Animator
}