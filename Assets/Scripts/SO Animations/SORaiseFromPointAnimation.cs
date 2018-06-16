using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Animations/Raise From Point")]
public class SORaiseFromPointAnimation : SOAnimation {
    public float speedMultiplier = 1f;
    public float positionTollerance = 0.01f;
    public Vector3 destination;
    public bool destinationIsActualPosition = true;
    public Vector3 startFrom;
    public int startFromMultiplier = 1;
    public bool startFromIsRelative = true;
    public Vector3 destinationScale;
    public float sizeTollerance = 0.01f;
    public bool alsoScale = true;

    protected override IEnumerator Animation(GameObject subject)
    {
        var localDestination = destination;

        if (destinationIsActualPosition)
        {
            localDestination = subject.transform.position;
        }

        if (startFromIsRelative)
        {
            subject.transform.position += startFrom * startFromMultiplier;
        }
        else
        {
            subject.transform.position = startFrom;
        }

        float counter = 0f;

        while (subject.transform.position != destination && (subject.transform.localScale != destinationScale && alsoScale))
        {
            counter += Time.deltaTime * speedMultiplier;

            subject.transform.position = Vector3.Lerp(subject.transform.position, localDestination, Mathf.Abs(Mathf.Sin(counter)));

            if (Vector3.Distance(subject.transform.position, destination) < positionTollerance)
                subject.transform.position = localDestination;

            if (alsoScale)
            {
                subject.transform.localScale = Vector3.Lerp(subject.transform.localScale, destinationScale, Mathf.Abs(Mathf.Sin(counter)));

                if (Vector3.Distance(subject.transform.localScale, destinationScale) < sizeTollerance)
                    subject.transform.localScale = destinationScale;
            }

            yield return new WaitForFixedUpdate();
        }

        Clean(subject);

        executeAtEnd.Invoke();
    }
}
