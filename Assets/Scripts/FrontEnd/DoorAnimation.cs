using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    protected int x;
    protected int y;
    private bool spawned;

    public float speed = 10f;

    IEnumerator despawnAnimation()
    {
        while (transform.localScale != Vector3.zero)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * speed);
            if (transform.localScale.x <= 0.01f)
                transform.localScale = Vector3.zero;
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator spawnAnimation()
    {
        while (transform.localScale != Vector3.one)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * speed);
            if (transform.localScale.x >= 0.99f)
                transform.localScale = Vector3.one;
            yield return new WaitForFixedUpdate();
        }
    }

    protected void Spawn()
    {
        spawned = true;
        StopCoroutine(despawnAnimation());
        StartCoroutine(spawnAnimation());
    }

    protected void Despawn()
    {
        spawned = false;
        StopCoroutine(spawnAnimation());
        StartCoroutine(despawnAnimation());
    }

    public void Think()
    {
        int wX = GameManager.witchInstance.GetX();
        int wY = GameManager.witchInstance.GetY();

        bool witch = (x == wX && y == wY);

        int pX = GameManager.playerInstance.getX();
        int pY = GameManager.playerInstance.getY();

        bool player = (x == pX && y == pY);

        if (witch || player)
        {
            Despawn();
        }
        else
        {
            Spawn();
        }
    }

    private void Start()
    {
        transform.localScale = Vector3.zero;
        spawned = false;
        x = GameManager.mapInstance.GetUscitaX();
        y = GameManager.mapInstance.GetUscitaY();
    }
}