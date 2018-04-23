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
                transform.localScale = new Vector3(1f, 3f, 1f);
                transform.GetChild(0).gameObject.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
                transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    IEnumerator moveTowards(Vector3 finalPos)
    {
        float counter = 0f;

        Vector3 originalPos = transform.position;

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

    public void InvokeMovement(float traQuanto)
    {
        if (GameManager.witchInstance.GetMosseFatte() < GameManager.witchInstance.GetMossePerTurno() && GameManager.turno == Turno.strega)
        {
            Invoke("MoveBackEnd", traQuanto);
        }
    }

}
