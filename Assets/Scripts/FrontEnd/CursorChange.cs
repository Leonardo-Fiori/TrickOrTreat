using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorChange : MonoBehaviour
{

    public Texture2D newTextureCursor;

 

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag != "Tile") return;
            TileMovement clickedTile = hit.transform.GetComponent<TileMovement>();
            //print("Effettivamente è una tile ");


            if((clickedTile.GetTileX() == GameManager.playerInstance.getX()) && (clickedTile.GetTileY() == GameManager.playerInstance.getY()))
            {
                //print("Tile del bambino");
                ChangeCursor();
            }
            
            else
            {
                ResetCursor();
            }
        }
    }
    // Update is called once per frame
    void ChangeCursor()
    {

        //print("Cambio cursore");
        Cursor.SetCursor(newTextureCursor, Vector2.zero, CursorMode.Auto);
    }

    private void ResetCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}