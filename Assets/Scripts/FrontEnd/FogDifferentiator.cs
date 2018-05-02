using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogDifferentiator : MonoBehaviour {

    private static int luciAttive = 0;
    public int maxLuciAttive = 2;
    public bool hasLights = true;

	// Use this for initialization
	void Start () {
        transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(-180f, 180f), 0));
        float scaleVariation = 0f;
        if (Random.Range(0, 1) == 1)
        {
            transform.localScale *= Random.Range(.8f, .99f);
        }
        else
        {
            transform.localScale *= Random.Range(1.1f, 1.2f);
        }

        if(hasLights == true)
        {
            GameObject luci = transform.GetChild(0).gameObject;

            if (Random.Range(0f, 100f) > 80f && luciAttive < maxLuciAttive && luci != null)
            {
                luci.SetActive(true);
                luciAttive++;
            }
            else
            {
                luci.SetActive(false);
            }
        }
    }
}
