using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    protected int x;
    protected int y;
    private bool spawned;

    public SOAnimation spawnAnim;
    public SOAnimation despawnAnim;

    protected void Spawn()
    {
        spawned = true;
        spawnAnim.Play(gameObject, this);
    }

    protected void Despawn()
    {
        spawned = false;
        despawnAnim.Play(gameObject, this);
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
        Think();
    }

    public void Initialize(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}