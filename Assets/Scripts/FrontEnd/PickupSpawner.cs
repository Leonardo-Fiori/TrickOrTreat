using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour {

    public GameObject prefabUscita;
    public GameObject prefabKey;
    public GameObject prefabCaramella;
    public GameObject prefabScarpetta;

    private GameObject uscita;
    private GameObject chiave;
    private GameObject scarpetta;
    private GameObject caramella;

    public GameObject GetPickup(string id)
    {
        if (id == "uscita")
            return uscita;

        if (id == "chiave")
            return chiave;

        if (id == "scarpetta")
            return scarpetta;

        if (id == "caramella")
            return caramella;

        return null;
    }

    int x;
    int y;

    void SpawnUscita()
    {
        uscita = Instantiate(prefabUscita, transform.position, Quaternion.identity);
    }

    void SpawnKey()
    {
        chiave = Instantiate(prefabKey, transform.position, Quaternion.identity);
        chiave.GetComponent<KeyAnimation>().Initialize(x, y);
    }

    void SpawnCaramella()
    {
        caramella = Instantiate(prefabCaramella, transform.position, Quaternion.identity);
        caramella.GetComponent<PickupAnimation>().Initialize(x, y);
    }

    void SpawnScarpetta()
    {
        scarpetta = Instantiate(prefabScarpetta, transform.position, Quaternion.identity);
        scarpetta.GetComponent<PickupAnimation>().Initialize(x, y);
    }

    void Start () {
        x = TileCoords.GetX(gameObject);
        y = TileCoords.GetY(gameObject);

        MapTile tile = GameManager.mapInstance.getTile(x, y);

        if (tile.IsUscita())
        {
            SpawnUscita();
        }

        if (tile.HasKey())
        {
            SpawnKey();
        }

        if (tile.HasCaramella())
        {
            SpawnCaramella();
        }

        if (tile.HasScarpetta())
        {
            SpawnScarpetta();
        }
    }
}
