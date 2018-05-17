using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaramellaAnimation : MonoBehaviour {
    private bool spawned;

    IEnumerator despawnAnimation()
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

    public SOEvent caramellaPresa;

    public void Think()
    {
        if(!TileHasFog() && !spawned)
            Spawn();

        if (GameManager.playerInstance.getX() == TileCoords.GetX(gameObject) && GameManager.playerInstance.getY() == TileCoords.GetY(gameObject))
        {
            Despawn();
            caramellaPresa.Raise();
        }
    }

    private bool TileHasFog()
    {
        return GameManager.GetFrontEndTile(TileCoords.GetX(gameObject), TileCoords.GetY(gameObject)).GetComponent<TileFog>().GetStatus();
    }

    public void Initialize(int x, int y)
    {
        TileCoords.SetX(gameObject, x);
        TileCoords.SetY(gameObject, y);
    }

    private void Start()
    {
        transform.localScale = Vector3.zero;
        spawned = false;
    }
}
