using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Event", menuName = "Event Holder", order = 4)]
public class SOEvent : ScriptableObject {
    public List<MonoBehaviour> listeners;

    public void Register(MonoBehaviour listener)
    {
        //Debug.Log(TileCoords.GetX(listener.gameObject));
        listeners.Add(listener);
    }

    public void Unregister(MonoBehaviour listener)
    {
        if (listeners.Contains(listener))
        {
            listeners.Remove(listener);
        }

    }

    public void Raise()
    {
        foreach(EventListener listener in listeners)
        {
            //Debug.Log(listener);
            listener.OnRaise();
        }
    }
}
