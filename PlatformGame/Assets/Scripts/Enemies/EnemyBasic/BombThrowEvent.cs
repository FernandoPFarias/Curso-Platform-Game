using UnityEngine;

public class BombThrowEvent : MonoBehaviour
{
    private Enemy enemy;

    private void Awake()
    {
        // Procura o Enemy no pai
        enemy = GetComponentInParent<Enemy>();
    }

    // Este método pode ser chamado pelo Animation Event
    public void ThrowBomb()
    {
        if (enemy != null)
        {
            enemy.GetBehaviour()?.Tick(enemy); // Garante que o estado está atualizado
            if (enemy.EnemyData is RangedEnemyData rangedData)
            {
                // Chama o método privado de disparo do projétil
                // Precisamos expor FireProjectile ou duplicar a lógica aqui
                // Alternativamente, podemos criar um método público em Enemy para disparar a bomba
                // Mas para manter simples, podemos instanciar diretamente aqui:
                var behaviour = enemy.GetBehaviour();
                var spawn = enemy.transform.Find("ProjectileSpawn");
                Vector3 spawnPos = spawn != null ? spawn.position : enemy.transform.position;
                Vector2 direction = (enemy.PlayerTarget.position - spawnPos).normalized;
                GameObject proj = GameObject.Instantiate(rangedData.projectilPrefab, spawnPos, Quaternion.identity);
                var bomb = proj.GetComponent<BombProjectile>();
                if (bomb != null)
                {
                    bomb.SetDamage(rangedData.projectileDamage);
                    bomb.LaunchArc(enemy.PlayerTarget.position);
                }
                else
                {
                    var projScript = proj.GetComponent<ProjectileBase>();
                    if (projScript != null)
                    {
                        projScript.SetDamage(rangedData.projectileDamage);
                        projScript.SetDirection(direction);
                    }
                }
            }
        }
    }
} 