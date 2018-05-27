using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyAnimation : MonoBehaviour
{

    protected int x;
    protected int y;
    private bool spawned;

    public SOEvent eventoChiavePresa;

    IEnumerator despawnAnimation()
    {
        while (transform.localScale.x >= 0.1f)
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

    public virtual void Think()
    {
        int wX = GameManager.playerInstance.getX();
        int wY = GameManager.playerInstance.getY();

        print(wX + " " + wY);

        if (x == wX && y == wY)
        {
            eventoChiavePresa.Raise();
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
