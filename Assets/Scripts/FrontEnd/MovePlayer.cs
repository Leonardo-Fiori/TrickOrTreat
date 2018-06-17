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
    public static bool moving;
    public float offsetY = 1f;
    public SOAnimation pawnMoveAnim;
    public SOAnimation warpAnim;

    /*
    public float raiseSpeed = 1f;
    public float raiseHeight = 3f;
    public float speed = 1f;
    public float jumpMultiplier = 1f;*/

    private void Start()
    {
        GameObject temp = new GameObject();
        temp.transform.position = transform.position + Vector3.back;
        transform.LookAt(temp.transform);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        Destroy(temp);
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
                MovePlayer.moving = true;
                //((SOPawnWarpAnimation)warpAnim).destination = destination;
                warpAnim.destination = destination;
                warpAnim.executeAtEnd.AddListener(AfterAnimationEnded);
                warpAnim.Play(gameObject, this);
            }
            else
            {
                MovePlayer.moving = true;
                //((SOPawnMoveAnimation)pawnMoveAnim).destination = destination;
                pawnMoveAnim.destination = destination;
                pawnMoveAnim.executeAtEnd.AddListener(AfterAnimationEnded);
                pawnMoveAnim.Play(gameObject, this);
            }
        }
    }

    public void AfterAnimationEnded()
    {
        GameManager.instance.eventoFineAnimazioneGiocatore.Raise();
        MovePlayer.moving = false;
        pawnMoveAnim.executeAtEnd.RemoveListener(AfterAnimationEnded);
    }
}
