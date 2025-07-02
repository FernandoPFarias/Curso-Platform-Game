using UnityEngine;
using System.Collections;

public class ShootingStarSpawnArea : MonoBehaviour
{
    [Header("Prefab e área de spawn")]
    [SerializeField] private ShootingStar shootingStarPrefab;
    [SerializeField] private Rect spawnArea = new Rect(-8, 2, 16, 4);

    [Header("Intervalo entre estrelas")]
    [SerializeField] private float minInterval = 2f;
    [SerializeField] private float maxInterval = 6f;

    [Header("Direção e velocidade")]
    [SerializeField] private float minSpeed = 2f;
    [SerializeField] private float maxSpeed = 4f;
    [SerializeField] private float minAngle = 200f;
    [SerializeField] private float maxAngle = 340f;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);

            float x = Random.Range(spawnArea.xMin, spawnArea.xMax);
            float y = Random.Range(spawnArea.yMin, spawnArea.yMax);

            float speed = Random.Range(minSpeed, maxSpeed);
            float angle = Random.Range(minAngle, maxAngle) * Mathf.Deg2Rad;
            Vector2 velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * speed;

            Vector3 spawnPos = new Vector3(x, y, 0f) + transform.position;

            ShootingStar star = Instantiate(shootingStarPrefab, spawnPos, Quaternion.identity, transform);
            star.Init(velocity, spawnArea);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 1f, 0.2f, 0.3f);
        Vector3 center = new Vector3(spawnArea.center.x, spawnArea.center.y, 0f) + transform.position;
        Vector3 size = new Vector3(spawnArea.size.x, spawnArea.size.y, 0.1f);
        Gizmos.DrawCube(center, size);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(center, size);
    }
} 