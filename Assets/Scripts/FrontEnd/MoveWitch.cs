using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWitch : MonoBehaviour {

    public float factor = 1f;
    public float offsetY = 1f;
    public static bool moving;
    public SOEvent eventEndAnim;
    public float speed = 1f;
    public float jumpMultiplier = 1f;

    private void Update()
    {
        // Pure debugging proposal, no jokes
        if(GameManager.debugMode == true)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                
            }
        }
    }

    IEnumerator moveTowards(Vector3 finalPos)
    {
        float counter = 0f;

        Vector3 originalPos = transform.position;

        // DA RIVEDERE, PROVA, ORIENTA LA STREGA VERSO DESTINAZIONE
        GameObject temp = new GameObject();
        temp.transform.position = finalPos;
        transform.LookAt(temp.transform);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        Destroy(temp);

        moving = true;

        // OLD MOVEMENT
        while (transform.position != finalPos)
        {
            counter += Time.deltaTime;

            float x = Mathf.Lerp(transform.position.x, finalPos.x, Mathf.Abs(Mathf.Sin(counter)));
            float z = Mathf.Lerp(transform.position.z, finalPos.z, Mathf.Abs(Mathf.Sin(counter)));

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

        eventEndAnim.Raise();

        /* // RAISE AND MOVE
        Vector3 upPos = transform.position + Vector3.up * jumpMultiplier;

        while(transform.position != upPos)
        {
            counter += Time.deltaTime * speed;

            transform.position = Vector3.Lerp(transform.position, upPos, Mathf.Abs(Mathf.Sin(counter)));

            if (Vector3.Distance(transform.position, upPos) < 0.01f)
            {
                transform.position = upPos;
            }

            yield return new WaitForFixedUpdate();
        }

        counter = 0f;

        Vector3 upFinalPos = finalPos + Vector3.up * jumpMultiplier;

        while (transform.position != upFinalPos)
        {
            counter += Time.deltaTime * speed;

            transform.position = Vector3.Lerp(transform.position, upFinalPos, Mathf.Abs(Mathf.Sin(counter)));

            if (Vector3.Distance(transform.position, upFinalPos) < 0.01f)
            {
                transform.position = upFinalPos;
            }

            yield return new WaitForFixedUpdate();
        }

        counter = 0f;

        while (transform.position != finalPos)
        {
            counter += Time.deltaTime * speed;

            transform.position = Vector3.Lerp(transform.position, finalPos, Mathf.Abs(Mathf.Sin(counter)));

            if (Vector3.Distance(transform.position, finalPos) < 0.01f)
            {
                transform.position = finalPos;
            }

            yield return new WaitForFixedUpdate();
        }*/

        // COME PEDINA
        /*
        Vector3 upPosition = originalPos + Vector3.up * jumpMultiplier;

        counter = 0f;

        float distanceOriginal = Vector3.Distance(new Vector3(transform.position.x, 0f, transform.position.z), finalPos);

        while (transform.position != finalPos)
        {
            counter += Time.deltaTime * speed;

            transform.position = Vector3.Lerp(transform.position, finalPos, Mathf.Abs(Mathf.Sin(counter)));

            if (Vector3.Distance(new Vector3(transform.position.x, 0f, transform.position.z), finalPos) >= distanceOriginal / 2f)
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
        }*/
    }

    public void Move(int x, int z, Movement mov)
    {
        if (mov == Movement.teleport)
        {
            transform.position = new Vector3(x * factor, offsetY, z * factor);
            return;
        }
        else if (mov == Movement.smooth)
        {
            StartCoroutine(moveTowards(new Vector3(x * factor, transform.position.y, z * factor)));
        }
    }

    private void MoveBackEnd()
    {
        GameManager.witchInstance.Move();
    }

    public void InvokeMovement()
    {
        if (GameManager.witchInstance.GetMosseFatte() < GameManager.witchInstance.GetMossePerTurno() && GameManager.turno == Turno.strega)
        {
            Invoke("MoveBackEnd", GameManager.instance.witchDelay);
        }
    }

}
