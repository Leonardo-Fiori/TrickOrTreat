using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAnimation : MonoBehaviour {

    int x;
    int y;
    public SOAnimation spawnAnim;
    public SOAnimation despawnAnim;
    public SOAnimation destroyAnim;

    public void Spawn()
    {
        spawnAnim.Play(gameObject, this);
    }

    public void Despawn()
    {
        despawnAnim.Play(gameObject, this);
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

        if (witch || player)
        {
            Despawn();
        }
    }

    public void Preso()
    {
        int pX = GameManager.playerInstance.getX();
        int pY = GameManager.playerInstance.getY();

        bool player = (x == pX && y == pY);

        // Non faccio ulteriori controlli come nella think perchè questa viene chiamata dall'evento specifico di pick
        if (player)
            destroyAnim.Play(gameObject, this);
    }
}
