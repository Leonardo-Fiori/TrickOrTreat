using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastManager : MonoBehaviour
{
    //public bool debugMode = false;
    private bool firstClick = false;

    public PlayerOnTileMovement playerPrefabRot = null;
    public MovePlayer playerPrefabMov = null;

    private void Start()
    {
        //playerPrefabRot = GameManager.playerPrefabInstance.GetComponent<PlayerOnTileMovement>();
        //playerPrefabMov = GameManager.playerPrefabInstance.GetComponent<MovePlayer>();
    }

    // dir va cambiato perchè mi fa sia da controllo sia da velocità di rotazione
    void Update()
    {
        if ((Input.GetMouseButtonUp(0) || (Input.GetMouseButtonUp(1))) && TileMovement.canRot && PlayerOnTileMovement.canRot && !MovePlayer.moving)
        {
            bool clockwise = false;

            int angleRot = 0;
            // dir va cambiato perchè mi fa sia da controllo sia da velocità di rotazione

            int dir = 0;
            if (Input.GetMouseButtonUp(0))
            {
                clockwise = true;
                angleRot = 90;
                dir = 2;
            }
            if (Input.GetMouseButtonUp(1))
            {
                clockwise = false;
                angleRot = -90;
                dir = -2;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {

                if (hit.transform.tag != "Tile") return;

                //print("raycast  dir" + dir);

                TileMovement clickedTile = hit.transform.GetComponent<TileMovement>();

                //print(clickedTile.GetTileX() + " " + clickedTile.GetTileY());

                bool equalX = (clickedTile.GetTileX() == GameManager.playerInstance.getX());
                bool equalY = (clickedTile.GetTileY() == GameManager.playerInstance.getY());

                MapTile tileBackEnd = GameManager.mapInstance.getTile(clickedTile.GetTileX(), clickedTile.GetTileY());

                //print(tileBackEnd.getTileType() + " " + tileBackEnd.getTileRotation());

                //return;

                // VUOLE RUOTARE, O E' IN MODALITA' DEBUG
                if ((equalX && equalY) || GameManager.debugMode == true)
                {
                    if(tileBackEnd.getTileType() != TileType.quadrivio)
                    {
                        clickedTile.StartTileRotation(angleRot, dir);

                        tileBackEnd.rotate(clockwise);

                        GameManager.playerInstance.IncrementaMosseFatte();
                    }

                    return;
                }

                // VUOLE MUOVERSI E NON E' IN MODALITA' DEBUG
                else if(!equalX || !equalY && GameManager.debugMode == false)
                {
                    // Ottengo la direzione di spostamento

                    int diffY = clickedTile.GetTileY() - GameManager.playerInstance.getY();
                    int diffX = clickedTile.GetTileX() - GameManager.playerInstance.getX();

                    Direction destDir = 0;

                    // Le tile più in alto rispetto alla cam hanno una Y più alta.

                    if(diffY != 0 && diffX == 0) // SI STA MUOVENDO VERTICALMENTE
                    {
                        if (diffY > 0 && diffY != 6) // Si sta spostando in su (es: 5-1 = 4)
                        {
                            destDir = Direction.nord;
                        }
                        else if (diffY < 0 && diffY != -6) // Si sta spostando in giu (es: 0-1 = -1)
                        {
                            destDir = Direction.sud;
                        }
                        else if (diffY == 6) // Si sta teletrasportando in su (6-0 = 6)
                        {
                            destDir = Direction.sud; // ma di conseguenza si muove in giù
                        }
                        else if (diffY == -6) // Si sta teletrasportando in giu (0-6 = -6)
                        {
                            destDir = Direction.nord; // ma di conseguenza si muove in su
                        }
                    }

                    else if(diffX != 0 && diffY == 0) // SI STA MUOVENDO ORIZZONTALMENTE
                    {
                        if (diffX > 0 && diffX != 6) // Si sta spostando a destra (es: 5-1 = 4)
                        {
                            destDir = Direction.est;
                        }
                        else if (diffX < 0 && diffX != -6) // Si sta spostando a sinistra (es: 0-1 = -1)
                        {
                            destDir = Direction.ovest;
                        }
                        else if (diffX == 6) // Si sta teletrasportando ad est (6-0 = 6)
                        {
                            destDir = Direction.ovest; // ma di conseguenza si muove ad ovest
                        }
                        else if (diffX == -6) // Si sta teletrasportando ad ovest (0-6 = -6)
                        {
                            destDir = Direction.est; // ma di conseguenza si muove ad est
                        }
                    }

                    // Controllo se il movimento è fattibile in quella direzione
                    if (GameManager.movementManagerInstance.canMove(destDir))
                    {
                        // Aggiorna in automatico il front end
                        GameManager.movementManagerInstance.movePlayer(destDir);
                    }
                }

            } // <- raycasthit
        } // <- getmousedown
    } // <- void update
}
