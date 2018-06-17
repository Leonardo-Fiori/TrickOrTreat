using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class SOAnimation : ScriptableObject {

    public float speedMultiplier = 1f;
    public float tollerance = 0.01f;
    public Vector3 destination;
    public float heightMultiplier;
    public Vector3 startFrom;
    public bool destinationIsActual = true;
    public bool startFromIsRelative = true;

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
    protected abstract IEnumerator Animation(GameObject subject, MonoBehaviour playOn);
    public bool Play(GameObject subject, MonoBehaviour playOn)
    {
        if (!Check(subject))
            return false;

        playOn.StartCoroutine(Animation(subject, playOn));

        return true;
    }
}