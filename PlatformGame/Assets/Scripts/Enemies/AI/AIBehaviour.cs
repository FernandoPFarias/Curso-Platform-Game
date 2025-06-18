using UnityEngine;

public abstract class AIBehaviour : ScriptableObject
{
    // Prepara o estado inicial do comportamento.
    public abstract void Initialize(Enemy puppet);

    // Chamado a cada ciclo de física pelo Enemy.cs. É o coração da IA.
    public abstract void Tick(Enemy puppet);

    // Chamado pelo EnemyHealth quando o inimigo toma dano.
    public virtual void OnTakeDamage(Enemy puppet) { }

    // Novo: método para desenhar gizmos customizados
    public virtual void DrawGizmos(Enemy enemy) { }
}