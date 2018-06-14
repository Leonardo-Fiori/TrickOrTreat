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
        float counter = 0f;
        
        while(fogPrefab.transform.localScale != Vector3.zero)
        {
            counter += Time.deltaTime;

            fogPrefab.transform.localScale = Vector3.Lerp(fogPrefab.transform.localScale, Vector3.zero, Mathf.Abs(Mathf.Sin(counter)));

            if (fogPrefab.transform.localScale.x < 0.01f)
                break;

            yield return new WaitForFixedUpdate();
        }

        //gameObject.GetComponentInChildren<ParticleSystem>().Play();

        fogPrefab.GetComponent<ParticleSystem>().Stop();

        fogPrefab.gameObject.SetActive(false);

        fogPrefab.SetActive(false);

        yield return null;
    }

    private void deactivateFog()
    {
        StartCoroutine(deactivateAnimation());

        if(!raised)
            //StartCoroutine(RaiseTile());

        //raised = true;

        StartCoroutine(InflateTile());

        SoundManager.instance.Play("tileappeared",0.25f);

        fogDisappearedEvent.Raise();

        return;
    }

    private void activateFog()
    {
        fogPrefab.SetActive(true);
        fogPrefab.GetComponent<ParticleSystem>().Play();
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

        while (transform.localScale != destination + Vector3.up)
        {
            counter += Time.deltaTime;

            transform.localScale = Vector3.Lerp(transform.localScale, destination + Vector3.up, Mathf.Abs(Mathf.Sin(counter)));

            if (Vector3.Distance(transform.localScale, destination + Vector3.up) < 0.001f)
                transform.localScale = destination + Vector3.up;

            yield return new WaitForFixedUpdate();
        }

        while (transform.localScale != destination)
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

        //gameObject.GetComponentInChildren<ParticleSystem>().Stop();

        fogPrefab = Instantiate(fogPrefab, transform);
	}

}
