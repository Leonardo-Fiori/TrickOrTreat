using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontEndTileManager : MonoBehaviour {
    private TileMovement[] tiles;
    public static FrontEndTileManager instance = null;

    void Start()
    {
        //print("ciaooo");
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
        tiles = FindObjectsOfType<TileMovement>();
    }

    public TileMovement GetFrontEndTile(int x, int y)
    {
        foreach(TileMovement tm in tiles)
        {
            if (tm.GetTileX() == x && tm.GetTileY() == y)
            {
                //print("trovato");
                return tm;
            }
        }

        Debug.Log("Front end tile manager: tile cercato non trovato");
        return null;
    }
}
