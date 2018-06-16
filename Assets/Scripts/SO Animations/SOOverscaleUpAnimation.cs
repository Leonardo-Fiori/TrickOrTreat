using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Animations/Overscale")]
public class SOOverscaleUpAnimation : SOAnimation {
    public float speedMultiplier = 1f;
    public float sizeTollerance = 0.01f;
    public Vector3 destinationSize;
    public Vector3 overscaleSize;

    protected override IEnumerator Animation(GameObject subject)
    {
        float counter = 0f;

        while (subject.transform.localScale != destinationSize + overscaleSize)
        {
            counter += Time.deltaTime;

            subject.transform.localScale = Vector3.Lerp(subject.transform.localScale, destinationSize + overscaleSize, Mathf.Abs(Mathf.Sin(counter)));

            if (Vector3.Distance(subject.transform.localScale, destinationSize + overscaleSize) < sizeTollerance)
                subject.transform.localScale = destinationSize + overscaleSize;

            yield return new WaitForFixedUpdate();
        }

        while (subject.transform.localScale != destinationSize)
        {
            counter += Time.deltaTime;

            subject.transform.localScale = Vector3.Lerp(subject.transform.localScale, destinationSize, Mathf.Abs(Mathf.Sin(counter)));

            if (Vector3.Distance(subject.transform.localScale, destinationSize) < sizeTollerance)
                subject.transform.localScale = destinationSize;

            yield return new WaitForFixedUpdate();
        }

        Clean(subject);

        executeAtEnd.Invoke();
    }
}
