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

    Vector3 startPosition;
    Vector3 cameraOffset;
    Camera cam;

    private GameObject fantoccio;

    private void Switch()
    {
        if (turno == Turno.strega)
        {
            turno = Turno.giocatore;
            subject = subject1;
        }
        else
        {
            turno = Turno.strega;
            subject = subject2;
        }
    }

    public void SwitchSubject()
    {
        Invoke("Switch", .5f);
    }

    private void Start()
    {
        turno = Turno.giocatore;
        subject = subject1;

        cam = GetComponent<Camera>();
        startPosition = transform.position;
        fantoccio = new GameObject();
        fantoccio.transform.position = subject.transform.position;
        cameraOffset = transform.position - fantoccio.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        
        // check per far ruotare la camera
        if (Input.GetMouseButton(2) && canRotate)
        {
            CameraRotation();
        }

        if (transform.position.y <= 2) transform.position = new Vector3(transform.position.x, 2.1f, transform.position.z);  // check perche la telecamera non rotei troppo in basso

        //transform.LookAt(subject.transform);
        fantoccio.transform.position = Vector3.Lerp(fantoccio.transform.position, subject.transform.position, Time.deltaTime * snappyness);
        transform.LookAt(fantoccio.transform);

        cam.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * scrollSpeed;
        if (cam.orthographicSize <= 0) cam.orthographicSize = 0.1f;
        if (cam.orthographicSize >= 6) cam.orthographicSize = 6f;

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
