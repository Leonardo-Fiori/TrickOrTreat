using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFog : MonoBehaviour {
    private int x;
    private int y;

    private bool active = true;

    public SOAnimation deactivateFogAnim;
    public SOAnimation spawnTileAnim;
    public SOAnimation secondarySpawnAnim;
    public bool useSecondarySpawnAnim = false;

    public GameObject fogPrefab;

    public bool GetStatus()
    {
        return active;
    }

    public void SetFog(bool status)
    {
        if (status == active) return;

        active = status;
        if (status == false) deactivateFog();
        else if (status == true) activateFog();
    }

    public void AfterFogDisabled()
    {
        fogPrefab.GetComponent<ParticleSystem>().Stop();

        fogPrefab.SetActive(false);

        deactivateFogAnim.executeAtEnd.RemoveListener(AfterFogDisabled);
    }

    public void AfterTileInflated()
    {
        GameObject uscita = gameObject.GetComponent<PickupSpawner>().GetPickup("uscita");
        if (uscita != null) uscita.gameObject.transform.parent = gameObject.transform;

        GameObject chiave = gameObject.GetComponent<PickupSpawner>().GetPickup("chiave");
        if (chiave != null) chiave.gameObject.transform.parent = gameObject.transform;

        deactivateFogAnim.executeAtEnd.RemoveListener(AfterTileInflated);
    }

    private void deactivateFog()
    {
        deactivateFogAnim.executeAtEnd.AddListener(AfterFogDisabled);
        deactivateFogAnim.Play(fogPrefab, this);

        if (useSecondarySpawnAnim)
        {
            secondarySpawnAnim.Play(gameObject, this);
        }

        spawnTileAnim.executeAtEnd.AddListener(AfterTileInflated);
        spawnTileAnim.Play(gameObject, this);

        GameManager.instance.eventoScomparsaNebbia.Raise();

        return;
    }

    private void activateFog()
    {
        fogPrefab.SetActive(true);
        fogPrefab.GetComponent<ParticleSystem>().Play();
        return;
    }

    // Spawna la nebbia
    void Start ()
    {     
        transform.localScale = new Vector3(0f, 0f, 0f);

        //gameObject.GetComponentInChildren<ParticleSystem>().Stop();

        fogPrefab = Instantiate(fogPrefab, transform);
	}

}
