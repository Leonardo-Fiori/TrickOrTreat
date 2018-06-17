using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWitch : MonoBehaviour {

    public float factor = 1f;
    public float offsetY = 1f;
    public static bool moving;
    public float speed = 1f;
    public float jumpMultiplier = 1f;
    public SOAnimation moveAnim;

    public void Move(int x, int z, Movement mov)
    {
        if (mov == Movement.teleport)
        {
            transform.position = new Vector3(x * factor, offsetY, z * factor);
            return;
        }
        else if (mov == Movement.smooth)
        {
            //StartCoroutine(moveTowards(new Vector3(x * factor, transform.position.y, z * factor)));
            moving = true;

            Vector3 finalPos = new Vector3(x * factor, transform.position.y, z * factor);

            // Look at destination
            GameObject temp = new GameObject();
            temp.transform.position = finalPos;
            transform.LookAt(temp.transform);
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
            Destroy(temp);

            // Anim
            //((SOPawnWarpAnimation)moveAnim).destination = finalPos;
            moveAnim.destination = finalPos;
            moveAnim.executeAtEnd.AddListener(AfterAnimationEnded);
            moveAnim.Play(gameObject,this);
        }
    }

    public void AfterAnimationEnded()
    {
        MoveWitch.moving = false;
        GameManager.instance.eventoFineAnimazioneStrega.Raise();
        moveAnim.executeAtEnd.RemoveListener(AfterAnimationEnded);
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
