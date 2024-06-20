using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventSystem : MonoBehaviour
{
    public static GameEventSystem instance;
    private void Awake()
    {
        instance = this;
    }


    Dictionary<EEventType, Action<object[]>> events = new Dictionary<EEventType, Action<object[]>>();

    public void Listen(EEventType type, Action<object[]> func)
    {
        if (!events.ContainsKey(type))
            events.Add(type, func);
        else
            events[type] += func;
    }

    public void Remove(EEventType type, Action<object[]> func)
    {
        if (events.ContainsKey(type))
            events[type] -= func;
    }

    public void Send(EEventType type, object[] objs)
    {
        events[type]?.Invoke(objs);
    }
}
