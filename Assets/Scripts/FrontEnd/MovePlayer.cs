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
    public SOEvent playerMovedEvent;
    public float speed = 1f;
    public float jumpMultiplier = 1f;

    private void Start()
    {
        GameObject temp = new GameObject();
        temp.transform.position = transform.position + Vector3.back;
        transform.LookAt(temp.transform);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        Destroy(temp);
    }

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

        Vector3 upPosition = transform.position + Vector3.up * jumpMultiplier;

        /* // EFFETTO PEDINA QUADRATO
        while(transform.position != upPosition)
        {
            counter += Time.deltaTime * 1f;

            transform.position = Vector3.Lerp(transform.position, upPosition, Mathf.Abs(Mathf.Sin(counter)));

            if (Vector3.Distance(transform.position, upPosition) < 0.001f)
                transform.position = upPosition;

            yield return new WaitForFixedUpdate();
        }

        counter = 0f;
        
        Vector3 upFinalPos = finalPos + Vector3.up;

        while (transform.position != upFinalPos)
        {
            counter += Time.deltaTime * 10f;

            transform.position = Vector3.Lerp(transform.position, upFinalPos, Mathf.Abs(Mathf.Sin(counter)));

            if (Vector3.Distance(transform.position, upFinalPos) < 0.001f)
                transform.position = upFinalPos;

            yield return new WaitForFixedUpdate();
        }

        counter = 0f;

        while (transform.position != finalPos)
        {
            counter += Time.deltaTime * 1f;

            transform.position = Vector3.Lerp(transform.position, finalPos, Mathf.Abs(Mathf.Sin(counter)));

            if (Vector3.Distance(transform.position, finalPos) < 0.001f)
                transform.position = finalPos;

            yield return new WaitForFixedUpdate();
        }
        */

        // PEDINA SMOOTH

        counter = 0f;

        float distanceOriginal = Vector3.Distance(new Vector3(transform.position.x, 0f, transform.position.z), finalPos);

        while (transform.position != finalPos)
        {
            counter += Time.deltaTime * speed;

            transform.position = Vector3.Lerp(transform.position, finalPos, Mathf.Abs(Mathf.Sin(counter)));

            if(Vector3.Distance(new Vector3(transform.position.x,0f,transform.position.z), finalPos) >= distanceOriginal/2f)
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, upPosition.y, transform.position.z), Mathf.Abs(Mathf.Sin(counter)));
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, originalPos.y, transform.position.z), Mathf.Abs(Mathf.Sin(counter)));
            }

            if (Vector3.Distance(transform.position, finalPos) < 0.01f)
                transform.position = finalPos;

            yield return new WaitForFixedUpdate();
        }

        moving = false;

        playerMovedEvent.Raise();
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

        playerMovedEvent.Raise();
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
