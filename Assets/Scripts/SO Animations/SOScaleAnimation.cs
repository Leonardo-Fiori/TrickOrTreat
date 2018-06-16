using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Animations/Scale")]
public class SOScaleAnimation : SOAnimation {
    public float speedMultiplier = 1f;
    public float sizeTollerance = 0.01f;
    public Vector3 destinationSize;

    protected override IEnumerator Animation(GameObject subject)
    {
        float timer = 0f;

        while(subject.transform.localScale != destinationSize)
        {
            timer += Time.deltaTime * speedMultiplier;

            subject.transform.localScale = Vector3.Lerp(subject.transform.localScale, destinationSize, Mathf.Abs(Mathf.Sin(timer)));

            if(Vector3.Distance(subject.transform.localScale, destinationSize) < sizeTollerance)
            {
                subject.transform.localScale = destinationSize;
            }

            yield return new WaitForFixedUpdate();
        }

        Clean(subject);

        executeAtEnd.Invoke();
    }
}
