using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour {

    public GameObject prefabUscita;
    public GameObject prefabKey;
    public GameObject[] prefabsCaramella;
    public GameObject prefabScarpetta;
    public GameObject prefabPetardo;

    public Vector3 verticalOffset;

    private GameObject uscita;
    private GameObject chiave;
    private GameObject scarpetta;
    private GameObject caramella;
    private GameObject petardo;

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

        if (id == "petardo")
            return petardo;

        return null;
    }

    int x;
    int y;

    void SpawnUscita()
    {
        uscita = Instantiate(prefabUscita, transform.position, Quaternion.identity);

        //uscita.transform.parent = gameObject.transform; // viene settato quando la fog despawna!!
    }

    void SpawnKey()
    {
        chiave = Instantiate(prefabKey, transform.position, Quaternion.identity);
        chiave.GetComponent<KeyAnimation>().Initialize(x, y);

        //chiave.transform.parent = gameObject.transform; // viene settato quando la fog despawna!!

        GameManager.mapInstance.getTile(x, y).SetChiaveFrontEnd(chiave);
    }

    private GameObject RandomCaramellaPrefab()
    {
        return prefabsCaramella[Random.Range(0, prefabsCaramella.Length)];
    }

    void SpawnCaramella()
    {
        caramella = Instantiate(RandomCaramellaPrefab(), transform.position + verticalOffset, Quaternion.identity);
        caramella.GetComponent<PickupAnimation>().Initialize(x, y);

        caramella.transform.parent = gameObject.transform;

        GameManager.mapInstance.getTile(x, y).SetCaramellaFrontEnd(caramella);
    }

    public void SpawnScarpetta()
    {
        scarpetta = Instantiate(prefabScarpetta, transform.position + verticalOffset, Quaternion.identity);
        scarpetta.GetComponent<PickupAnimation>().Initialize(x, y);

        scarpetta.transform.parent = gameObject.transform;

        GameManager.mapInstance.getTile(x, y).SetScarpettaFrontEnd(scarpetta); 
    }

    public void SpawnPetardo()
    {
        petardo = Instantiate(prefabPetardo, transform.position + verticalOffset, Quaternion.identity);
        petardo.GetComponent<PickupAnimation>().Initialize(x, y);

        petardo.transform.parent = gameObject.transform;

        GameManager.mapInstance.getTile(x, y).SetPetardoFrontEnd(petardo);
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

        if (tile.HasPetardo())
        {
            //print("Ho un petardo ("+x+" "+y+")");
            SpawnPetardo();
        }
    }
}
