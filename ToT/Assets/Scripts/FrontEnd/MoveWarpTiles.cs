using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWarpTiles : MonoBehaviour {
    private int x = 0;
    private int y = 0;
    private Vector3 originalPos;
    int playerX;
    int playerY;
    int dim;

    private void Start()
    {
        originalPos = transform.position;
        x = GetComponent<TileMovement>().GetTileX();
        y = GetComponent<TileMovement>().GetTileY();
        dim = GameManager.mapInstance.dim - 1;
    }

    public void WarpToMe()
    {
        GameObject[] oppositeTile = null;
        int dim = GameManager.mapInstance.dim;
        dim--;
        //print(dim);
        //return;

        oppositeTile = new GameObject[2];
        oppositeTile[0] = null;
        oppositeTile[1] = null;

        if (x == 0 && y == 0) // si trova in angolo in basso a sx
        {
            oppositeTile[0] = GameManager.frontEndTileManagerInstance.GetFrontEndTile(dim, 0).gameObject; // tile da muovere a sx
            oppositeTile[0].GetComponent<MoveWarpTiles>().WarpMeTo(transform.position, Rotation.sinistra);
            oppositeTile[1] = GameManager.frontEndTileManagerInstance.GetFrontEndTile(0, dim).gameObject; // tile da muovere sotto
            oppositeTile[1].GetComponent<MoveWarpTiles>().WarpMeTo(transform.position, Rotation.giu);
        }
        else if(x == dim && y == dim) // si trova in angolo in alto a dx
        {
            oppositeTile[0] = GameManager.frontEndTileManagerInstance.GetFrontEndTile(0, dim).gameObject; // tile da muovere a dx
            oppositeTile[0].GetComponent<MoveWarpTiles>().WarpMeTo(transform.position, Rotation.destra);
            oppositeTile[1] = GameManager.frontEndTileManagerInstance.GetFrontEndTile(dim, 0).gameObject; // tile da muovere sopra
            oppositeTile[1].GetComponent<MoveWarpTiles>().WarpMeTo(transform.position, Rotation.su);
        }
        else if(x == 0 && y == dim) // si trova in angolo in alto a sx
        {
            oppositeTile[0] = GameManager.frontEndTileManagerInstance.GetFrontEndTile(dim, dim).gameObject; // tile da muovere a sx
            oppositeTile[0].GetComponent<MoveWarpTiles>().WarpMeTo(transform.position, Rotation.sinistra);
            oppositeTile[1] = GameManager.frontEndTileManagerInstance.GetFrontEndTile(0, 0).gameObject; // tile da muovere sopra
            oppositeTile[1].GetComponent<MoveWarpTiles>().WarpMeTo(transform.position, Rotation.su);
        }
        else if(x == dim && y == 0) // si trova in angolo in basso a dx
        {
            oppositeTile[0] = GameManager.frontEndTileManagerInstance.GetFrontEndTile(0, 0).gameObject; // tile da muovere a dx
            oppositeTile[0].GetComponent<MoveWarpTiles>().WarpMeTo(transform.position, Rotation.destra);
            oppositeTile[1] = GameManager.frontEndTileManagerInstance.GetFrontEndTile(dim, dim).gameObject; // tile da muovere sotto
            oppositeTile[1].GetComponent<MoveWarpTiles>().WarpMeTo(transform.position, Rotation.giu);
        }
        else if(x == 0 && y >= 0 && y <= dim) // si trova lungo il lato sinistro
        {
            oppositeTile[0] = GameManager.frontEndTileManagerInstance.GetFrontEndTile(dim, y).gameObject;
            oppositeTile[0].GetComponent<MoveWarpTiles>().WarpMeTo(transform.position, Rotation.sinistra);
        }
        else if(x == dim && y >= 0 && y <= dim) // si trova lungo il lato destro
        {
            oppositeTile[0] = GameManager.frontEndTileManagerInstance.GetFrontEndTile(0, y).gameObject;
            oppositeTile[0].GetComponent<MoveWarpTiles>().WarpMeTo(transform.position, Rotation.destra);
        }
        else if(y == 0 && x >= 0 && x <= dim) // si trova lungo la base
        {
            oppositeTile[0] = GameManager.frontEndTileManagerInstance.GetFrontEndTile(x, dim).gameObject;
            oppositeTile[0].GetComponent<MoveWarpTiles>().WarpMeTo(transform.position, Rotation.giu);
        }
        else if(y == dim && x >= 0 && x <= dim) // si trova lungo il lato in alto
        {
            oppositeTile[0] = GameManager.frontEndTileManagerInstance.GetFrontEndTile(x, 0).gameObject;
            oppositeTile[0].GetComponent<MoveWarpTiles>().WarpMeTo(transform.position, Rotation.su);
        }
        
    }

    public void WarpMeTo(Vector3 destinationTilePos, Rotation rot)
    {
        transform.position = destinationTilePos;

        if(rot == Rotation.su)
        {
            transform.position += Vector3.forward;
        }
        else if (rot == Rotation.giu)
        {
            transform.position += Vector3.back;
        }
        else if (rot == Rotation.destra)
        {
            transform.position += Vector3.right;
        }
        else if (rot == Rotation.sinistra)
        {
            transform.position += Vector3.left;
        }

        Invoke("WarpMeBack", 3f);
    }

    private void WarpMeBack()
    {
        transform.position = originalPos;
    }
}
