using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Data/Slime Enemy")]
public class SlimeData : MeleeEnemyData // Herda de Melee, pois o Slime ataca de perto
{
    [Header("Slime Specifics")]
    public float hopForce;         // A for�a do pulo de patrulha
    public float hopPauseDuration; // O tempo que ele espera no ch�o entre os pulos
}