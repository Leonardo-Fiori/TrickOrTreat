using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFog : MonoBehaviour {

    public bool active = true;
    public GameObject fogPrefab;
    public GameObject spotLight;

    private float lightIntensity;
    private Light lightComponent;

    public void SetFog(bool status)
    {
        active = status;
        if (status == false) deactivateFog();
        else if (status == true) activateFog();
    }

    private IEnumerator deactivateAnimation()
    {
        while(fogPrefab.transform.localScale.x > 0.1f)
        {
            fogPrefab.transform.localScale = fogPrefab.transform.localScale * .9f;
            if (lightComponent.intensity < lightIntensity)
                lightComponent.intensity += 0.1f;
            yield return null;
        }
        fogPrefab.GetComponent<ParticleSystem>().Stop();
        fogPrefab.SetActive(false);
    }

    private void deactivateFog()
    {
        StartCoroutine(deactivateAnimation());
        return;
    }

    private void activateFog()
    {
        fogPrefab.SetActive(true);
        fogPrefab.GetComponent<ParticleSystem>().Play();
        return;
    }

    // Spawna la nebbia
	void Start () {
        fogPrefab = Instantiate(fogPrefab, transform);
        spotLight = Instantiate(spotLight, transform);

        lightComponent = spotLight.GetComponent<Light>();
        lightIntensity = lightComponent.intensity;
        lightComponent.intensity = 0f;
	}

}
