using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAnimation : MonoBehaviour {

    int x;
    int y;
    public float speed = 10f;

    IEnumerator despawnAnimation()
    {
        while (transform.localScale != Vector3.zero)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * speed);
            if (transform.localScale.x <= 0.01f)
                transform.localScale = Vector3.zero;
            yield return null;
        }
    }

    IEnumerator destroyAnimation()
    {
        while (transform.localScale != Vector3.zero)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * speed);
            if (transform.localScale.x <= 0.01f)
                transform.localScale = Vector3.zero;
            yield return new WaitForFixedUpdate();
        }

        Destroy(gameObject);
    }

    IEnumerator spawnAnimation()
    {
        while (transform.localScale != Vector3.one)
        {
            print("spawn");
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * speed);
            if (transform.localScale.x >= 0.99f)
                transform.localScale = Vector3.one;
            yield return new WaitForFixedUpdate();
        }
    }

    public void Spawn()
    {
        StopCoroutine(despawnAnimation());
        StartCoroutine(spawnAnimation());
    }

    public void Despawn()
    {
        StopCoroutine(spawnAnimation());
        StartCoroutine(despawnAnimation());
    }

    public void Initialize(int xx, int yy)
    {
        x = xx;
        y = yy;
    }

    private void Start()
    {
        transform.localScale = Vector3.zero;
        Think();
    }

    public void Think()
    {
        int wX = GameManager.witchInstance.GetX();
        int wY = GameManager.witchInstance.GetY();

        bool witch = (x == wX && y == wY);

        int pX = GameManager.playerInstance.getX();
        int pY = GameManager.playerInstance.getY();

        bool player = (x == pX && y == pY);

        bool fog = GameManager.mapInstance.getTile(x, y).getPrefab().GetComponent<TileFog>().GetStatus();

        if (!player && !witch && !fog)
        {
            Spawn();
        }

        if (witch)
        {
            Despawn();
        }

        if (player)
        {
            StartCoroutine(destroyAnimation());
        }
    }
}
