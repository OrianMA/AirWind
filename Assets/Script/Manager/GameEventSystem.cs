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


    Dictionary<EEventType, UnityEvent<object[]>> events = new Dictionary<EEventType,UnityEvent<object[]>>();

    public void Listen(EEventType type, Func<object[]> func)
    {
        //events[type].AddListener();
    }

    public void Remove(EEventType type, Func<object[]> func)
    {

    }

    void Send(EEventType type)
    {
        //events[type].Invoke();
    }
}
