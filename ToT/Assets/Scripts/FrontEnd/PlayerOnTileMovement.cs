using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnTileMovement : MonoBehaviour {

    public float height = 1f;
    public float speed = 1f;
    public float returnSpeed = 1f;
    int posizioneFinale = 0;
    int incremento = 0;

    public float secondsInAir = 1f;

    public static bool canRot = true;

    public IEnumerator PlayerRotation(int angleRot, int dir)
    {
        canRot = false;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + Vector3.up * height;

        // la tile sale
        while (Vector3.Distance(transform.position, endPosition) > 0.5f)
        {
            transform.position = Vector3.Lerp(transform.position, endPosition, speed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(secondsInAir);

        // Torna in posizione
        while (Vector3.Distance(transform.position, startPosition) >= 0.1)
        {
            transform.position = Vector3.Lerp(transform.position, startPosition, returnSpeed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

        transform.position = Vector3.MoveTowards(transform.position, startPosition, 1);
        canRot = true;
    }
}