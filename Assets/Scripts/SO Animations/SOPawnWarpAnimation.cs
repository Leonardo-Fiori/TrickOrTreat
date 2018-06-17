using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Animations/Pawn Warp")]
public class SOPawnWarpAnimation : SOAnimation {

    /*
    public float speedMultiplier = 1f;
    public float tollerance = 0.01f;
    public float heightMultiplier = 0f;
    public Vector3 destination;*/

    protected override IEnumerator Animation(GameObject subject, MonoBehaviour playOn)
    {
        Vector3 moveTo = base.destination;
        float heightMult = base.heightMultiplier;
        Vector3 endPosition = subject.transform.position + (Vector3.up * heightMult);
        float speed = base.speedMultiplier;

        float counter = 0f;

        while (subject.transform.position != endPosition)
        {
            counter += Time.deltaTime * speed;

            float y = Mathf.Lerp(subject.transform.position.y, endPosition.y, Mathf.Abs(Mathf.Sin(counter)));

            Vector3 newTransform = new Vector3(subject.transform.position.x, y, subject.transform.position.z);

            if (Mathf.Abs(Vector3.Distance(newTransform, endPosition)) <= tollerance)
            {
                subject.transform.position = endPosition;
            }
            else
            {
                subject.transform.position = newTransform;
            }

            yield return new WaitForFixedUpdate();
        }

        moveTo.y = endPosition.y;
        endPosition = moveTo;

        counter = 0f;

        while (subject.transform.position != endPosition)
        {
            counter += Time.deltaTime * speed;

            float y = Mathf.Lerp(subject.transform.position.y, endPosition.y, counter);

            Vector3 newTransform = Vector3.Lerp(subject.transform.position, endPosition, Mathf.Abs(Mathf.Sin(counter)));

            if (Mathf.Abs(Vector3.Distance(newTransform, endPosition)) <= tollerance)
            {
                subject.transform.position = endPosition;
            }
            else
            {
                subject.transform.position = newTransform;
            }

            yield return new WaitForFixedUpdate();
        }

        endPosition = subject.transform.position + (Vector3.down * heightMult);

        counter = 0f;

        while (subject.transform.position != endPosition)
        {
            counter += Time.deltaTime * speed;

            float y = Mathf.Lerp(subject.transform.position.y, endPosition.y, counter);

            Vector3 newTransform = Vector3.Lerp(subject.transform.position, endPosition, Mathf.Abs(Mathf.Sin(counter)));

            if (Mathf.Abs(Vector3.Distance(newTransform, endPosition)) <= tollerance)
            {
                subject.transform.position = endPosition;
            }
            else
            {
                subject.transform.position = newTransform;
            }

            yield return new WaitForFixedUpdate();
        }

        Clean(subject);

        executeAtEnd.Invoke();
    }
}
