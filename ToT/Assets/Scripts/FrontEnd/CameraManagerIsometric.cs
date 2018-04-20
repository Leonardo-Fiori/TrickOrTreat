using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManagerIsometric : MonoBehaviour
{

    public GameObject player;
    public float scrollSpeed = 10f;
    public float cameraDistance = 5f;

    Vector3 startPosition;
    Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        transform.LookAt(player.transform);


        // Da migliorare! Se la camera si avvicina troppo si blocca, questo è un tappo temporaneo per non farla buggare
        if (transform.position == player.transform.position)
        {
            transform.position = startPosition;
        }

        cam.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * scrollSpeed;
        if (cam.orthographicSize <= 0) cam.orthographicSize = 0.1f;

    }
}
