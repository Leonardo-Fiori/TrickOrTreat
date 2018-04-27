using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHghlight : MonoBehaviour {
    MeshRenderer mr;
    Color originalColor;
    Color destinationColor;
    TileFog tf;

    private void Start()
    {
        tf = GetComponent<TileFog>();
        mr = GetComponent<MeshRenderer>();
        originalColor = mr.material.color;
        destinationColor = mr.material.color + Color.red;
    }

    private void OnMouseOver()
    {
        if(!tf.GetStatus())
            mr.material.color = destinationColor;
    }

    private void OnMouseExit()
    {
        mr.material.color = originalColor;
    }
}
