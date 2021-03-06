﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesInDanger : MonoBehaviour {
    public List<MapTile> tilesInDanger;
    public bool mouseOverWitch;
    public bool toggled = false;

    private void OnMouseEnter()
    {
        if (mouseOverWitch)
            return;

        mouseOverWitch = true;

        GameManager.instance.eventoHoveringStrega.Raise();
        //SoundManager.instance.Play("tilehover");
    }

    private void OnMouseExit()
    {
        mouseOverWitch = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(GameManager.controls.standardClick) && mouseOverWitch)
        {
            toggled = !toggled;

            if (!toggled)
            {
                GameManager.instance.eventoWitchToggled.Raise();
                //SoundManager.instance.Play("clicksimple");
            }
            else
            {
                GameManager.instance.eventoWitchUntoggled.Raise();
                //SoundManager.instance.Play("clickrisata");
            }
        }
    }

    public void OnWitchMove()
    {
        int witchX = GameManager.witchInstance.GetX();
        int witchY = GameManager.witchInstance.GetY();

        int mosseStrega = GameManager.witchInstance.GetMossePerTurno() - GameManager.witchInstance.GetMosseFatte();

        tilesInDanger.Clear();

        // sulla x della strega
        tilesInDanger = GameManager.movementManagerInstance.getNextTiles(witchX, witchY, Direction.nord, mosseStrega);
        tilesInDanger.AddRange(GameManager.movementManagerInstance.getNextTiles(witchX, witchY, Direction.sud, mosseStrega));
        tilesInDanger.AddRange(GameManager.movementManagerInstance.getNextTiles(witchX, witchY, Direction.est, mosseStrega));
        tilesInDanger.AddRange(GameManager.movementManagerInstance.getNextTiles(witchX, witchY, Direction.ovest, mosseStrega));

        for(int i = 1; i < mosseStrega; i++)
        {
            int nextX = (witchX - i) % GameManager.movementManagerInstance.map.dimensione;
            if(nextX == -1) nextX = GameManager.movementManagerInstance.map.dimensione - 1;
            tilesInDanger.AddRange(GameManager.movementManagerInstance.getNextTiles(nextX, witchY, Direction.nord, mosseStrega - i));
            tilesInDanger.AddRange(GameManager.movementManagerInstance.getNextTiles(nextX, witchY, Direction.sud, mosseStrega - i));
        }

        for (int i = 1; i < mosseStrega; i++)
        {
            int nextX = (witchX + i) % GameManager.movementManagerInstance.map.dimensione;
            tilesInDanger.AddRange(GameManager.movementManagerInstance.getNextTiles(nextX, witchY, Direction.nord, mosseStrega - i));
            tilesInDanger.AddRange(GameManager.movementManagerInstance.getNextTiles(nextX, witchY, Direction.sud, mosseStrega - i));
        }

        //print(tilesInDanger.Count);

        GameManager.instance.eventoTilePericoloseAggiornate.Raise();
    }

    private void Start()
    {
        tilesInDanger = new List<MapTile>();
        Invoke("OnWitchMove", 1f);
    }
}
