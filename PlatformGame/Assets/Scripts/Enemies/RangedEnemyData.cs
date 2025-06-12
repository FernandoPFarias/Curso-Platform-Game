using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "EnemyData/Ranged Enemy")]
public class RangedEnemyData : EnemyData
{
    [Header("Ranged Attack Status")]
    public GameObject projectilPrefab; // o projetil que o inimigo joga
    public float fireRate; // quantos tiros por segundo
    public float visionRange; // distancia que ele ve o player

}
