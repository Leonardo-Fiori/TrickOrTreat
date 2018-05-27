using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWitch : MonoBehaviour {

    public float factor = 1f;
    public float offsetY = 1f;
    public static bool moving;
    public SOEvent eventEndAnim;

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
        while (transform.position != finalPos)
        {
            counter += Time.deltaTime;

            float x = Mathf.Lerp(transform.position.x, finalPos.x, counter * 2f);
            float z = Mathf.Lerp(transform.position.z, finalPos.z, counter * 2f);

            Vector3 newTransform = new Vector3(x, transform.position.y, z);

            if (Mathf.Abs(Vector3.Distance(transform.position, newTransform)) <= 0.01f)
            {
                transform.position = finalPos;
            }
            else
            {
                transform.position = newTransform;
            }

            yield return new WaitForEndOfFrame();
        }
        moving = false;

        eventEndAnim.Raise();
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
