using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private Dictionary<Type, List<Delegate>> eventTable;

    private void Awake()
    {
        eventTable = new Dictionary<Type, List<Delegate>>();
    }

    public void Subscribe<T>(Action<T> listener)
    {
        Type t = typeof(T);
        if (!eventTable.ContainsKey(t))
            eventTable[t] = new List<Delegate>();
        eventTable[t].Add(listener);
    }

    public void Fire<T>(T gameEvent)
    {
        Type t = typeof(T);
        if (!eventTable.ContainsKey(t)) return;

        for (int i = eventTable[t].Count - 1; i >= 0; i--)
        {
            if (eventTable[t][i] is Action<T> action)
                action.Invoke(gameEvent);
        }
    }
}

public struct WaveStartedEvent { public int waveNumber; }
public struct WaveCompletedEvent { public int waveNumber; }
public struct EnemyKilledEvent { public int remainingEnemies; }
public struct ScoreChangedEvent { public int score; }
public struct GameOverEvent { }
