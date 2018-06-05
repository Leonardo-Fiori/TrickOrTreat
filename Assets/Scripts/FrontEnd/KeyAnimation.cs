using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyAnimation : MonoBehaviour
{

    protected int x;
    protected int y;
    private bool spawned;

    public float speed = 10f;

    public SOEvent eventoChiavePresa;

    IEnumerator despawnAnimation()
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

    IEnumerator despawnAnimationNoDestroy()
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

    public void Spawn()
    {
        spawned = true;
        StopCoroutine(despawnAnimation());
        StartCoroutine(spawnAnimation());
    }

    public void Despawn()
    {
        spawned = false;
        StopCoroutine(spawnAnimation());
        StartCoroutine(despawnAnimation());
    }

    public void DespawnNoDestroy()
    {
        spawned = false;
        StopCoroutine(spawnAnimation());
        StartCoroutine(despawnAnimationNoDestroy());
    }

    public void Think()
    {
        int wX = GameManager.playerInstance.getX();
        int wY = GameManager.playerInstance.getY();

        //print(wX + " " + wY);

        if (x == wX && y == wY)
        {
            Despawn();
        }
    }

    private void Start()
    {
        transform.localScale = Vector3.zero;
        Spawn();
    }

    public void Initialize(int xx, int yy)
    {
        x = xx;
        y = yy;
    }
}
