using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Animations/Scale for Fog Particle")]
public class SOFogScaleAnimation : SOAnimation {
    protected override IEnumerator Animation(GameObject subject, MonoBehaviour playOn)
    {
        float timer = 0f;

        /*
        while(subject.transform.localScale != destination)
        {
            timer += Time.deltaTime * speedMultiplier;

            subject.transform.localScale *= .9f;

            if(Mathf.Abs(subject.transform.localScale.x - destination.x) < tollerance)
            {
                subject.transform.localScale = destination;
            }

            subject.transform.localScale = destination;

            yield return new WaitForFixedUpdate();
        }
        */

        Clean(subject);

        executeAtEnd.Invoke();

        yield return null;
    }
}
