using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFog : MonoBehaviour {
    private int x;
    private int y;

    private bool raised = false;
    private bool active = true;

    public GameObject fogPrefab;

    public SOEvent fogDisappearedEvent;

    //public GameObject spotLight;
    //private float lightIntensity;
    //private Light lightComponent;

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

    private IEnumerator deactivateAnimation()
    {
        while(fogPrefab.transform.localScale.x > 0.1f)
        {
            fogPrefab.transform.localScale = fogPrefab.transform.localScale * .9f;
            yield return new WaitForFixedUpdate();
        }
        fogPrefab.GetComponentInChildren<ParticleSystem>().Stop();
        fogPrefab.SetActive(false);
        //transform.localScale = new Vector3(1f, 1f, 1f);
    }

    private void deactivateFog()
    {
        StartCoroutine(deactivateAnimation());

        if(!raised)
            StartCoroutine(RaiseTile());

        raised = true;

        StartCoroutine(InflateTile());

        SoundManager.instance.Play("tileappeared",0.25f);

        fogDisappearedEvent.Raise();

        return;
    }

    private void activateFog()
    {
        fogPrefab.SetActive(true);
        fogPrefab.GetComponentInChildren<ParticleSystem>().Play();
        return;
    }

    public IEnumerator RaiseTile()
    {
        raised = true;
        Vector3 destination = transform.position;
        transform.position += Vector3.down * 10;

        float counter = 0f;
        
        while(transform.position != destination)
        {
            counter += Time.deltaTime;

            transform.position = Vector3.Lerp(transform.position, destination, Mathf.Abs(Mathf.Sin(counter)));

            if (Vector3.Distance(transform.position, destination) < 0.001f)
                transform.position = destination;

            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator InflateTile()
    {
        Vector3 destination = new Vector3(1f, 1f, 1f);

        float counter = 0f;

        while (transform.localScale.x != destination.x)
        {
            counter += Time.deltaTime;

            transform.localScale = Vector3.Lerp(transform.localScale, destination, Mathf.Abs(Mathf.Sin(counter)));

            if (Vector3.Distance(transform.localScale, destination) < 0.001f)
                transform.localScale = destination;

            yield return new WaitForFixedUpdate();
        }

        GameObject uscita = gameObject.GetComponent<PickupSpawner>().GetPickup("uscita");
        if(uscita != null) uscita.gameObject.transform.parent = gameObject.transform;

        GameObject chiave = gameObject.GetComponent<PickupSpawner>().GetPickup("chiave");
        if(chiave != null) chiave.gameObject.transform.parent = gameObject.transform;
    }

    // Spawna la nebbia
    void Start ()
    {     
        transform.localScale = new Vector3(0f, 0f, 0f);

        fogPrefab = Instantiate(fogPrefab, transform);
	}

}
