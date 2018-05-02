﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManagerIsometric : MonoBehaviour
{

    public GameObject subject;
    public float scrollSpeed = 10f;
    public float cameraDistance = 5f;

    public float snappyness = 10f;

    Vector3 startPosition;
    Camera cam;

    private GameObject fantoccio;

    private void Start()
    {
        cam = GetComponent<Camera>();
        startPosition = transform.position;
        fantoccio = new GameObject();
        fantoccio.transform.position = subject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        //transform.LookAt(subject.transform);
        fantoccio.transform.position = Vector3.Lerp(fantoccio.transform.position, subject.transform.position, Time.deltaTime * snappyness);
        transform.LookAt(fantoccio.transform);


        // Da migliorare! Se la camera si avvicina troppo si blocca, questo è un tappo temporaneo per non farla buggare
        if (transform.position == subject.transform.position)
        {
            transform.position = startPosition;
        }

        cam.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * scrollSpeed;
        if (cam.orthographicSize <= 0) cam.orthographicSize = 0.1f;

    }
}
