using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvent", menuName = "SO/Variable/GameEvent", order = 0)]
public class GameEvent : ScriptableObject
{
    private List<GameEventListener> listeners = new List<GameEventListener>();

    public void Subscribe(GameEventListener listener)
    {
        if (!listeners.Contains(listener))
            listeners.Add(listener);
    }

    public void UnSubscribe(GameEventListener listener)
    {
        if (listeners.Contains(listener))
            listeners.Remove(listener);
    }

    public void Raise()
    {
        foreach (var i in listeners)
        {
            i.Fire();
        }
    }
}