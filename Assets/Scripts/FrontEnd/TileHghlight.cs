﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHghlight : MonoBehaviour {
    MeshRenderer mr;
    Color originalColor;
    Color destinationColor;
    TileFog tf;

    private void Start()
    {
        tf = GetComponent<TileFog>();
        mr = GetComponent<MeshRenderer>();
        originalColor = mr.material.color;
        destinationColor = mr.material.color + Color.red;
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
            if(on || sud && canMoveSud || est && canMoveEst || ovest && canMoveOvest || nord && canMoveNord)
                mr.material.color = destinationColor;
    }

    private void OnMouseExit()
    {
        mr.material.color = originalColor;
    }
}