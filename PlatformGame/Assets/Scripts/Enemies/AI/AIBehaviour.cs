using UnityEngine;

public class AIBehaviour : ScriptableObject
{
    public virtual void Initialize(Enemy puppet) { }
    public virtual void Tick(Enemy puppet) { }

    // MÉTODO FALTANTE ADICIONADO AQUI:
    // Retorna 'true' se o jogador foi detectado, 'false' caso contrário.
    // É 'virtual' para que comportamentos específicos como o SentryBehaviour possam sobrescrevê-lo.
    public virtual bool IsPlayerDetected(Enemy puppet)
    {
        if (puppet.PlayerTarget == null) return false;

        // Comportamento padrão de detecção: um círculo simples.
        float distanceToPlayer = Vector2.Distance(puppet.transform.position, puppet.PlayerTarget.position);
        return distanceToPlayer < puppet.EnemyData.detectionRange;
    }
}