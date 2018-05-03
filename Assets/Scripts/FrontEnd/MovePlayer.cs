using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Script che fa muovere il prefab del giocatore in una data direzione quando viene chiamata la move()
 * NON FA CONTROLLI SE LO SPOSTAMENTO E' LECITO O MENO. I controlli vengono fatti nel back-end.
 * Questo è uno script del front-end.
*/

public class MovePlayer : MonoBehaviour {

    // Di quante coordinate si deve spostare?
    public float factor = 1f;
    public float offsetY = 1f;
    public float raiseSpeed = 1f;
    public float raiseHeight = 3f;
    public static bool moving;

    // Sposta il prefab del giocatore di tot factor in direzione dir

    IEnumerator moveTowards(Vector3 finalPos)
    {
        moving = true;
        GameObject temp = new GameObject();
        temp.transform.position = finalPos;
        transform.LookAt(temp.transform);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        Destroy(temp);

        float counter = 0f;

        Vector3 originalPos = transform.position;

        while (transform.position != finalPos)
        {
            counter += Time.deltaTime;

            float x = Mathf.Lerp(transform.position.x, finalPos.x, counter * 2f);//Mathf.MoveTowards(transform.position.x, finalPos.x, speed);
            float z = Mathf.Lerp(transform.position.z, finalPos.z, counter * 2f);//Mathf.MoveTowards(transform.position.z, finalPos.z, speed);

            Vector3 newTransform = new Vector3(x, transform.position.y, z);

            if (Mathf.Abs(Vector3.Distance(transform.position, newTransform)) <= 0.01f)
            {
                transform.position = finalPos;
            }
            else
            {
                transform.position = newTransform;
            }

            yield return new WaitForFixedUpdate();
        }
        moving = false;
    }

    IEnumerator warpTowards(Vector3 destination)
    {
        moving = true;

        Vector3 endPosition = transform.position + (Vector3.up * 2f);

        float counter = 0f;

        while (transform.position != endPosition)
        {
            counter += Time.deltaTime;

            float y = Mathf.Lerp(transform.position.y, endPosition.y, counter);

            Vector3 newTransform = new Vector3(transform.position.x, y, transform.position.z);

            if (Mathf.Abs(Vector3.Distance(newTransform, endPosition)) <= 0.01f)
            {
                transform.position = endPosition;
            }
            else
            {
                transform.position = newTransform;
            }

            yield return new WaitForFixedUpdate();
        }

        destination.y = endPosition.y;
        endPosition = destination;

        counter = 0f;

        while (transform.position != endPosition)
        {
            counter += Time.deltaTime;

            float y = Mathf.Lerp(transform.position.y, endPosition.y, counter);

            Vector3 newTransform = Vector3.Lerp(transform.position, endPosition, counter);

            if (Mathf.Abs(Vector3.Distance(newTransform, endPosition)) <= 0.01f)
            {
                transform.position = endPosition;
            }
            else
            {
                transform.position = newTransform;
            }

            yield return new WaitForFixedUpdate();
        }

        endPosition = transform.position + (Vector3.down * 2f);

        counter = 0f;

        while (transform.position != endPosition)
        {
            counter += Time.deltaTime;

            float y = Mathf.Lerp(transform.position.y, endPosition.y, counter);

            Vector3 newTransform = Vector3.Lerp(transform.position, endPosition, counter);

            if (Mathf.Abs(Vector3.Distance(newTransform, endPosition)) <= 0.01f)
            {
                transform.position = endPosition;
            }
            else
            {
                transform.position = newTransform;
            }

            yield return new WaitForFixedUpdate();
        }

        moving = false;
    }

    public void move(int x, int z, Movement mov)
    {
        if (mov == Movement.teleport)
        {
            transform.position = new Vector3(x * factor, offsetY, z * factor);
            return;
        }
        else if(mov == Movement.smooth)
        {
            Vector3 destination = new Vector3(x * factor, transform.position.y, z * factor);

            if(Mathf.Abs(Vector3.Distance(destination,transform.position)) >= 2 * factor)
            {
                StartCoroutine(warpTowards(destination));
            }
            else
            {
                StartCoroutine(moveTowards(destination));
            }
        }
    }
}
