using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class TileMovement : MonoBehaviour
{
    private int tileX = 0;
    private int tileY = 0;

    public float height = 1f;
    public float speed = 1f;
    public float returnSpeed = 1f;
    public float rotationSpeed = 10f;

    public static bool canRot = true;

    public void StartTileRotation(int angleRot, int dir)
    {
        StartCoroutine(TileRotation(angleRot, dir));
    }

    public int GetTileX()
    {
        return tileX;
    }
    public int GetTileY()
    {
        return tileY;
    }
    public void SetTileX(int x)
    {
        tileX = x;
    }
    public void SetTileY(int y)
    {
        tileY = y;
    }

    //Animazione Tile
    IEnumerator TileRotation(int angleRot, int dir) 
    {
        canRot = false;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + Vector3.up * height;

        // la tile sale
        while (Vector3.Distance(transform.position, endPosition) > 0.5f)
        {
            // print("Primo while");

            transform.position = Vector3.Lerp(transform.position, endPosition, speed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(0.1f);

        for(float i = 0; i < rotationSpeed; i++)
        {
            transform.Rotate(0f, angleRot / rotationSpeed, 0f);
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(0.1f);

        // Torna in posizione
        while (Vector3.Distance(transform.position, startPosition) >= 0.1)
        {
            //print("while di ritorno");
            transform.position = Vector3.Lerp(transform.position, startPosition, returnSpeed * Time.deltaTime);
            yield return new WaitForFixedUpdate();

        }

        transform.position = Vector3.MoveTowards(transform.position, startPosition, 1);
        canRot = true;
    }
}