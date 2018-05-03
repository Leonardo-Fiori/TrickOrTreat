using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawnExit : MonoBehaviour {

    public GameObject prefabUscita;

    void SpawnUscita()
    {
        Instantiate(prefabUscita, transform, false);
    }

	void Start () {
        MapTile tile = GameManager.mapInstance.getTile(TileCoords.GetX(gameObject),TileCoords.GetY(gameObject));

        if (tile.IsUscita())
        {
            SpawnUscita();
        }
	}
	
}
