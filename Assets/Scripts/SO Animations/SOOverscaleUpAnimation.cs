using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Animations/Overscale")]
public class SOOverscaleUpAnimation : SOAnimation {
    /*public float speedMultiplier = 1f;
    public float tollerance = 0.01f;
    public Vector3 destination;*/

    protected override IEnumerator Animation(GameObject subject, MonoBehaviour playOn)
    {
        float counter = 0f;

        Vector3 overscaleSize = new Vector3(0f, destination.y, 0f);

        while (subject.transform.localScale != destination + overscaleSize)
        {
            counter += Time.deltaTime;

            subject.transform.localScale = Vector3.Lerp(subject.transform.localScale, destination + overscaleSize, Mathf.Abs(Mathf.Sin(counter)));

            if (Vector3.Distance(subject.transform.localScale, destination + overscaleSize) < tollerance)
                subject.transform.localScale = destination + overscaleSize;

            yield return new WaitForFixedUpdate();
        }

        while (subject.transform.localScale != destination)
        {
            counter += Time.deltaTime;

            subject.transform.localScale = Vector3.Lerp(subject.transform.localScale, destination, Mathf.Abs(Mathf.Sin(counter)));

            if (Vector3.Distance(subject.transform.localScale, destination) < tollerance)
                subject.transform.localScale = destination;

            yield return new WaitForFixedUpdate();
        }

        Clean(subject);

        executeAtEnd.Invoke();
    }
}
