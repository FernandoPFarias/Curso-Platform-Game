using UnityEngine;

// O contrato que todo estado de IA deve seguir.
// Não herda de MonoBehaviour.
public abstract class AIState
{
    protected Enemy stateMachine;

    public AIState(Enemy enemy)
    {
        stateMachine = enemy;
    }

    public virtual void OnEnter() { }
    public virtual void Tick() { }
    public virtual void OnExit() { }
}