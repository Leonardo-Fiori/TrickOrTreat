using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Animations/Scale with Bounce")]
public class SOScaleBounceAnimation : SOAnimation {

    protected override IEnumerator Animation(GameObject subject, MonoBehaviour playOn)
    {
        float timer = 0f;

        Vector3 inflatedBounce = subject.transform.localScale + Vector3.one;

        while (subject.transform.localScale != inflatedBounce)
        {
            timer += Time.deltaTime * speedMultiplier * 2;

            subject.transform.localScale = Vector3.Lerp(subject.transform.localScale, inflatedBounce, Mathf.Abs(Mathf.Sin(timer)));

            if (Vector3.Distance(subject.transform.localScale, inflatedBounce) < tollerance)
            {
                subject.transform.localScale = inflatedBounce;
            }

            yield return new WaitForFixedUpdate();
        }

        timer = 0f;

        while (subject.transform.localScale != destination)
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
