using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAnimation : MonoBehaviour {

    int x;
    int y;

    IEnumerator despawnAnimation()
    {
        while (transform.localScale.x >= 0.01f)
        {
            transform.localScale *= 0.9f;
            yield return null;
        }
    }

    IEnumerator destroyAnimation()
    {
        while (transform.localScale.x >= 0.01f)
        {
            transform.localScale *= 0.9f;
            yield return null;
        }

        Destroy(gameObject);
    }

    IEnumerator spawnAnimation()
    {
        while (transform.localScale.x <= 1f)
        {
            transform.localScale += Vector3.one * 0.1f;

            if (Vector3.Distance(transform.localScale, Vector3.one) <= 0.1f)
            {
                transform.localScale = Vector3.one;
            }

            yield return null;
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
        transform.localScale = new Vector3(.1f, .1f, .1f);
        Despawn();
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
