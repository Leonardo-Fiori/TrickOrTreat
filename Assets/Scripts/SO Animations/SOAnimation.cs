using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class SOAnimation : ScriptableObject {
    public static List<GameObject> animatingOn;
    public UnityEvent executeAtEnd;
    protected bool Check(GameObject subject)
    {
        if (animatingOn == null)
            animatingOn = new List<GameObject>();

        if (animatingOn.Contains(subject))
        {
            return false;
        }
        else
        {
            animatingOn.Add(subject);
            return true;
        }
    }
    protected void Clean(GameObject subject)
    {
        if (animatingOn.Contains(subject))
            animatingOn.Remove(subject);
    }
    protected abstract IEnumerator Animation(GameObject subject);
    public bool Play(GameObject subject, MonoBehaviour playOn)
    {
        if (!Check(subject))
            return false;

        playOn.StartCoroutine(Animation(subject));

        return true;
    }
}