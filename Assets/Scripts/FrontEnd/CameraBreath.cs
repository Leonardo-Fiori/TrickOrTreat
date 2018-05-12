using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBreath : MonoBehaviour {

    Camera cam = null;
    float angle = 0;
    public float speed;
    public float magnitude;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        angle += Time.deltaTime * speed;
        if (angle > 360) angle = 0;

        float value = Mathf.Sin(angle) * magnitude;

        cam.orthographicSize += value;
    }
}
