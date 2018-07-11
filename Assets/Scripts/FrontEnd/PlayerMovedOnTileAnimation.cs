using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovedOnTileAnimation : MonoBehaviour {

    public SOAnimation tileAnim;
    public static bool animating = false;
    int x;
    int y;

    private void Start()
    {
        x = TileCoords.GetX(gameObject);
        y = TileCoords.GetY(gameObject);
    }

    public void Think()
    {
        if(GameManager.playerInstance.getX() == x && GameManager.playerInstance.getY() == y)
        {
            tileAnim.Play(gameObject,this);
        }
    }
}
