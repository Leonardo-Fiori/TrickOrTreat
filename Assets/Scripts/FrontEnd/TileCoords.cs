using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCoords : MonoBehaviour {

    [SerializeField] private int x = 0;
    [SerializeField] private int y = 0;

    public static void SetX(GameObject go, int newX)
    {
        go.GetComponent<TileCoords>().x = newX;
    }

    public static int GetX(GameObject go)
    {
        return go.GetComponent<TileCoords>().x;
    }

    public static void SetY(GameObject go, int newY)
    {
        go.GetComponent<TileCoords>().y = newY;
    }

    public static int GetY(GameObject go)
    {
        return go.GetComponent<TileCoords>().y;
    }

}
