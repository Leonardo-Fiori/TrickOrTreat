using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHghlight : MonoBehaviour {
    public Color witchDangerColor;
    public Color hilightColor;
    public float multiplier = 0.5f;

    MeshRenderer mr;
    TileFog tf;
    TilesInDanger tiles;
    private bool mouseOver = false;
    private bool canMoveHere = false;
    private bool inDanger = false;

    private void Update()
    {
        Color dest = Color.black;

        if (canMoveHere && mouseOver)
        {
            dest += hilightColor;
        }

        if (inDanger && (tiles.mouseOverWitch || tiles.toggled || GameManager.turno == Turno.strega))
        {
            dest += witchDangerColor;
        }

        mr.material.SetColor("_EmissionColor", dest * multiplier);
    }

    private void Start()
    {
        tf = GetComponent<TileFog>();
        mr = GetComponent<MeshRenderer>();
        tiles = GameManager.witchPrefabInstance.GetComponent<TilesInDanger>();
    }

    public void OnDangerousTilesUpdated()
    {
        int x = TileCoords.GetX(gameObject);
        int y = TileCoords.GetY(gameObject);

        List<MapTile> tilesInDanger = tiles.tilesInDanger;

        foreach(MapTile tile in tilesInDanger)
        {
            if(tile.getPrefab() == this.gameObject)
            {
                inDanger = true;
                break;
            }
            else
            {
                inDanger = false;
            }
        }
    }

    private void OnMouseOver()
    {
        int playerX = GameManager.playerInstance.getX();
        int playerY = GameManager.playerInstance.getY();
        int x = TileCoords.GetX(gameObject);
        int y = TileCoords.GetY(gameObject);

        bool on = (playerX == x && playerY == y);
        bool nord = (GameManager.movementManagerInstance.getNextTile(playerX, playerY, Direction.nord).getPrefab() == gameObject);
        bool sud = (GameManager.movementManagerInstance.getNextTile(playerX, playerY, Direction.sud).getPrefab() == gameObject);
        bool est = (GameManager.movementManagerInstance.getNextTile(playerX, playerY, Direction.est).getPrefab() == gameObject);
        bool ovest = (GameManager.movementManagerInstance.getNextTile(playerX, playerY, Direction.ovest).getPrefab() == gameObject);

        bool canMoveNord = GameManager.movementManagerInstance.canMove(Direction.nord);
        bool canMoveSud = GameManager.movementManagerInstance.canMove(Direction.sud);
        bool canMoveEst = GameManager.movementManagerInstance.canMove(Direction.est);
        bool canMoveOvest = GameManager.movementManagerInstance.canMove(Direction.ovest);

        if (!tf.GetStatus())
        {
            if (on || sud && canMoveSud || est && canMoveEst || ovest && canMoveOvest || nord && canMoveNord)
            {
                canMoveHere = true;
            }
            else
            {
                canMoveHere = false;
            }
        }

        mouseOver = true;
    }

    private void OnMouseExit()
    {
        mouseOver = false;
    }
}
