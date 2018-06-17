using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Animations/Pawn Move")]
public class SOPawnMoveAnimation : SOAnimation {/*
    public float speedMultiplier = 1f;
    public float tollerance = 0.01f;
    public float jumpHeightMultiplier;
    public Vector3 destination;*/

    protected override IEnumerator Animation(GameObject subject, MonoBehaviour playOn)
    {
        Vector3 finalPos = destination;
        float jumpMultiplier = heightMultiplier;
        float speed = speedMultiplier;

        GameObject temp = new GameObject();
        temp.transform.position = finalPos;
        subject.transform.LookAt(temp.transform);
        subject.transform.rotation = Quaternion.Euler(0f, subject.transform.rotation.eulerAngles.y, 0f);
        Destroy(temp);

        float counter = 0f;

        Vector3 originalPos = subject.transform.position;

        Vector3 upPosition = subject.transform.position + Vector3.up * jumpMultiplier;

        float distanceOriginal = Vector3.Distance(new Vector3(subject.transform.position.x, 0f, subject.transform.position.z), finalPos);

        while (subject.transform.position != finalPos)
        {
            counter += Time.deltaTime * speed;

            subject.transform.position = Vector3.Lerp(subject.transform.position, finalPos, Mathf.Abs(Mathf.Sin(counter)));

            if (Vector3.Distance(new Vector3(subject.transform.position.x, 0f, subject.transform.position.z), finalPos) >= distanceOriginal / 2f)
            {
                subject.transform.position = Vector3.Lerp(subject.transform.position, new Vector3(subject.transform.position.x, upPosition.y, subject.transform.position.z), Mathf.Abs(Mathf.Sin(counter)));
            }
            else
            {
                subject.transform.position = Vector3.Lerp(subject.transform.position, new Vector3(subject.transform.position.x, originalPos.y, subject.transform.position.z), Mathf.Abs(Mathf.Sin(counter)));
            }

            if (Vector3.Distance(subject.transform.position, finalPos) < 0.01f)
                subject.transform.position = finalPos;

            yield return new WaitForFixedUpdate();
        }

        executeAtEnd.Invoke();

        Clean(subject);
    }
}
