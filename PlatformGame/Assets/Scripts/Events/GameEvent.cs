using UnityEngine;
using System;
using System.Collections.Generic;

// Verifique esta linha. É ela que cria o item no menu.
[CreateAssetMenu(menuName = "Game Event Channel")]
public class GameEvent : ScriptableObject 
{
    private readonly List<Action> listeners = new List<Action>();

    public void Raise()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].Invoke();
        }
    }

    public void RegisterListener(Action listener)
    {
        if (!listeners.Contains(listener))
        {
            listeners.Add(listener);
        }
    }

    public void UnregisterListener(Action listener)
    {
        if (listeners.Contains(listener))
        {
            listeners.Remove(listener);
        }
    }
}