using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Animations/Player Moved On Tile")]
public class SOPlayerMovedOnTileAnimation : SOAnimation {

    protected override IEnumerator Animation(GameObject subject, MonoBehaviour playOn)
    {
        PlayerMovedOnTileAnimation.animating = true;

        float timer = 0f;

        Vector3 original = subject.transform.localScale;
        Vector3 inflatedBounce = subject.transform.localScale + Vector3.up;
        Vector3 downPos = subject.transform.position + Vector3.down;
        Vector3 originalPos = subject.transform.position;

        while (subject.transform.localScale != inflatedBounce)
        {
            timer += Time.deltaTime * speedMultiplier * 2;

            subject.transform.localScale = Vector3.Lerp(subject.transform.localScale, inflatedBounce, Mathf.Abs(Mathf.Sin(timer)));

            //subject.transform.position = Vector3.Lerp(subject.transform.position, downPos, Mathf.Abs(Mathf.Sin(timer)));

            if (Vector3.Distance(subject.transform.localScale, inflatedBounce) < tollerance)
            {
                subject.transform.localScale = inflatedBounce;
            }

            yield return new WaitForFixedUpdate();
        }

        timer = 0f;

        while (subject.transform.localScale != original)
        {
            timer += Time.deltaTime * speedMultiplier;

            subject.transform.localScale = Vector3.Lerp(subject.transform.localScale, original, Mathf.Abs(Mathf.Sin(timer)));

            //subject.transform.position = Vector3.Lerp(subject.transform.position, originalPos, Mathf.Abs(Mathf.Sin(timer)));

            if (Vector3.Distance(subject.transform.localScale, original) < tollerance)
            {
                subject.transform.localScale = original;
            }

            yield return new WaitForFixedUpdate();
        }

        Clean(subject);

        executeAtEnd.Invoke();

        PlayerMovedOnTileAnimation.animating = false;
    }
}
