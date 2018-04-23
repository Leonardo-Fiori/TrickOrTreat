using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventListener : MonoBehaviour {
    public SOEvent eventToListen;
    public UnityEvent whatToDo;

    private void OnEnable()
    {
        eventToListen.Register(this);
    }

    private void OnDisable()
    {
        eventToListen.Unregister(this);
    }

    public void OnRaise()
    {
        whatToDo.Invoke();
    }
}
