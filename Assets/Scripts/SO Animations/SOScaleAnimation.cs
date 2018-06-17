using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Animations/Scale")]
public class SOScaleAnimation : SOAnimation {/*
    public float speedMultiplier = 1f;
    public float tollerance = 0.01f;
    public Vector3 destination;*/

    protected override IEnumerator Animation(GameObject subject, MonoBehaviour playOn)
    {
        float timer = 0f;

        while(subject.transform.localScale != destination)
        {
            timer += Time.deltaTime * speedMultiplier;

            subject.transform.localScale = Vector3.Lerp(subject.transform.localScale, destination, Mathf.Abs(Mathf.Sin(timer)));

            if(Vector3.Distance(subject.transform.localScale, destination) < tollerance)
            {
                subject.transform.localScale = destination;
            }

            yield return new WaitForFixedUpdate();
        }

        Clean(subject);

        executeAtEnd.Invoke();
    }
}
