using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorEmission : MonoBehaviour {

    MeshRenderer mr;
    Material material;
    int chiavi = 0;
    int chiaviMassime;

    private void Start()
    {
        chiaviMassime = GameManager.mapInstance.GetQuanteChiavi();
        mr = GetComponentInChildren<MeshRenderer>();
        material = mr.material;
        material.SetColor("_EmissionColor", Color.black);
    }

    public void Think()
    {
        chiavi++;

        if (chiavi < chiaviMassime)
            material.SetColor("_EmissionColor", (Color.white * (chiavi / (2f * chiaviMassime))));
        else
            material.SetColor("_EmissionColor", Color.white * (chiavi / chiaviMassime));
    }
}
