using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatAnimation : MonoBehaviour {

    public float ampiezza = 0.001f;
    public float speed = 1f;
    public bool rotazione = true;
    public float speedX = 0f;
    public float speedY = 1f;
    public float speedZ = 0f;
    public bool randomizeSpeed = true;

    private IEnumerator Float()
    {
        while (true)
        {
            transform.position += Vector3.up * Mathf.Sin(Time.realtimeSinceStartup * speed) * ampiezza;
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator Rotation()
    {
        while (true)
        {
            transform.Rotate(speedX, speedY, speedZ);
            yield return new WaitForFixedUpdate();
        }
    }

    private void Start()
    {
        if (randomizeSpeed)
        {
            speed = Random.Range(speed / 2f, speed + (speed / 2f));
            speedX = Random.Range(speedX / 2f, speedX + (speedX / 2f));
            speedY = Random.Range(speedY / 2f, speedY + (speedY / 2f));
            speedZ = Random.Range(speedZ / 2f, speedZ + (speedZ / 2f));
        }

        StartCoroutine(Float());

        if (rotazione)
            StartCoroutine(Rotation());
    }

}
