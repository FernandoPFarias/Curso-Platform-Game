using UnityEngine;

public abstract class AIState 
{
    protected Enemy stateMachine;

    public AIState(Enemy enemy)
    { 
     stateMachine = enemy;   
    }


    // M�todos que cada estado pode implementar
    public virtual void OnEnter() { }  // Chamado quando entramos neste estado
    public virtual void Tick() { }     // Chamado a cada frame (como o Update)
    public virtual void OnExit() { }   // Chamado quando sa�mos deste estado


}
