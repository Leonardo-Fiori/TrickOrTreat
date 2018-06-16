using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManagerIsometric : MonoBehaviour
{

    public GameObject subject1;
    public GameObject subject2;
    private GameObject subject;
    private Turno turno;
    public float scrollSpeed = 10f;
    public float cameraDistance = 5f;
    public float rotationSpeed = 1f;
    
    public bool canRotate = true;
    public bool canUpDown = false;  // sconsiglio caldamente l'attivazione ( buggatino)
    public float snappyness = 10f;
    public float minZoom = 6f;
    public float maxZoom = 2f;

    Vector3 startPosition;
    Vector3 cameraOffset;
    Camera cam;

    private GameObject center;
    private GameObject fantoccio;

    public void SwitchSubject()
    {
        print("SWITCH!");
        if (GameManager.turno == Turno.strega)
        {
            subject = subject2;
            turno = Turno.giocatore;
        }
        else
        {
            subject = subject1;
            turno = Turno.strega;
        }
    }

    private void Start()
    {
        subject = subject1;

        cam = GetComponent<Camera>();
        startPosition = transform.position;
        fantoccio = new GameObject();
        fantoccio.transform.position = subject.transform.position;
        cameraOffset = transform.position - fantoccio.transform.position;

        int dim = GameManager.mapInstance.dimensione;
        center = GameManager.mapInstance.getTile(dim / 2, dim / 2).getPrefab();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        
        // Ruota la camera
        if (Input.GetKey(GameManager.controls.rotazioneCamera) && canRotate)
        {
            CameraRotation();
        }

        // Clamp rotazione camera
        if (transform.position.y <= 2)
            transform.position = new Vector3(transform.position.x, 2.1f, transform.position.z);

        // Camera follow
        CameraFollow();

        // Zoom
        CameraZoom();
    }

    void CameraFollow()
    {
        // Trovo la posizione tra centro e soggetto
        Vector3 destination = subject.transform.position + ((center.transform.position - subject.transform.position) / 2);

        //Vector3 destination = subject.transform.position;

        fantoccio.transform.position = Vector3.Lerp(fantoccio.transform.position, destination, Time.deltaTime * snappyness);

        //print(subject);

        transform.LookAt(fantoccio.transform);
    }

    void CameraZoom()
    {
        if(Input.GetKey(GameManager.controls.zoomCamera))
            cam.orthographicSize -= Input.GetAxis("Mouse Y") * Time.deltaTime * scrollSpeed / 10f;

        cam.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * scrollSpeed;
        if (cam.orthographicSize <= maxZoom) cam.orthographicSize = maxZoom;
        if (cam.orthographicSize >= minZoom) cam.orthographicSize = minZoom;
    }

    void CameraRotation()
    {
        
        Quaternion camTurnY = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotationSpeed, Vector3.up);   // ruota la camera in base alla posizione del mouse
        
        if (canUpDown)
        {
            Quaternion camTurnX = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * rotationSpeed, Vector3.right);   // questo è quello per farla andare su e giù
            cameraOffset = camTurnY * camTurnX * cameraOffset;  // usa questa se voglio poter roteare anche su o giu (booleana modificabile solo da editor)
        }
        else
        {
            cameraOffset = camTurnY * cameraOffset; // altrimenti roteo solo intorno al giocatore
        }

        Vector3 newPos = fantoccio.transform.position + cameraOffset;

        transform.position = Vector3.Lerp(transform.position, newPos, rotationSpeed);

        /*
        transform.RotateAround(fantoccio.transform.position, Vector3.up, rotationSpeed * Time.deltaTime * Input.GetAxis("Mouse X"));
        transform.RotateAround(fantoccio.transform.position, Vector3.forward, -(rotationSpeed * Time.deltaTime * Input.GetAxis("Mouse Y"))/4f);
        transform.RotateAround(fantoccio.transform.position, Vector3.back, (rotationSpeed * Time.deltaTime * Input.GetAxis("Mouse Y"))/2f);
        transform.RotateAround(fantoccio.transform.position, Vector3.right, -(rotationSpeed * Time.deltaTime * Input.GetAxis("Mouse Y"))/4f);
        transform.RotateAround(fantoccio.transform.position, Vector3.left, (rotationSpeed * Time.deltaTime * Input.GetAxis("Mouse Y"))/4f);
        print(transform.rotation.eulerAngles.x);
        if (transform.rotation.eulerAngles.x <= 360 && transform.rotation.eulerAngles.x >= 350)
        {
            print("ciao");
            transform.rotation = Quaternion.Euler(new Vector3(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
        }

        //print(Input.GetAxis("Mouse X"));*/
    }
}
