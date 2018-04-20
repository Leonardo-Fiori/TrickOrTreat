using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    public GameObject player;
    public float scrollSpeed = 10f;
    public float cameraDistance = 5f;

    Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update () {

        transform.LookAt(player.transform);


        // Da migliorare! Se la camera si avvicina troppo si blocca, questo è un tappo temporaneo per non farla buggare
        if (transform.position == player.transform.position)
        {
            transform.position = startPosition;
        }
        
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * scrollSpeed);
        

	}
}
