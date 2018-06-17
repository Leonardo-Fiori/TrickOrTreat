using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Animations/Raise From Point")]
public class SORaiseFromPointAnimation : SOAnimation {
    /*
    public float speedMultiplier = 1f;
    public float tollerance = 0.01f;
    public Vector3 destination;
    public Vector3 startFrom;
    public bool destinationIsActual = true;
    public bool startFromIsRelative = true;*/

    protected override IEnumerator Animation(GameObject subject, MonoBehaviour playOn)
    {
        var localDestination = destination;

        if (destinationIsActual)
        {
            localDestination = subject.transform.position;
        }

        if (startFromIsRelative)
        {
            subject.transform.position += startFrom;
        }
        else
        {
            subject.transform.position = startFrom;
        }

        float counter = 0f;

        while (subject.transform.position != destination)
        {
            counter += Time.deltaTime * speedMultiplier;

            subject.transform.position = Vector3.Lerp(subject.transform.position, localDestination, Mathf.Abs(Mathf.Sin(counter)));

            if (Vector3.Distance(subject.transform.position, destination) < tollerance)
                subject.transform.position = localDestination;

            yield return new WaitForFixedUpdate();
        }

        Clean(subject);

        executeAtEnd.Invoke();
    }
}
