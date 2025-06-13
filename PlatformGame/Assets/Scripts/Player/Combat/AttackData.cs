using UnityEngine;



public abstract class AttackData : ScriptableObject
{
    [Header("Attack Info")]
    public string attackType;
    public float attackdamage;
    public float attackCooldown;


    [Header("Animation & Effects")]
    public string animationTrrigerName; // Nome do gatilho no animator
    public GameEvent onAttackSfx;      // Canal do evento para o SOM
    public GameEvent onAttackVfx;     // Canal do evento para as particulas

    [Header("HitBox")]
    public Vector2 hitboxOffSet; // posição da hitbox
    public Vector2 hitboxSize;  // tamanho do circulo da box
    public LayerMask damageableLayers; // Definir quais as layer que podem tomar dano




    public abstract void Execute(GameObject attacker);

}
