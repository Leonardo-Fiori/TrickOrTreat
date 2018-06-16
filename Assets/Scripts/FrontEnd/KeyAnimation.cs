using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyAnimation : MonoBehaviour
{

    protected int x;
    protected int y;
    private bool spawned;

    public SOAnimation spawnAnim;
    public SOAnimation despawnAnim;
    public SOAnimation destroyAnim;

    public void Spawn()
    {
        spawned = true;
        spawnAnim.Play(gameObject, this);
    }

    public void Despawn()
    {
        spawned = false;
        destroyAnim.Play(gameObject, this);
    }

    public void DespawnNoDestroy()
    {
        spawned = false;
        despawnAnim.Play(gameObject, this);
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
