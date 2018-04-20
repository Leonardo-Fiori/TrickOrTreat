using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Richiede una mappa e un giocatore che gli verranno passati da il gameManager.
 * Gestisce gli spostamenti sulla mappa, facendo i relativi controlli.
 * E' instanziato dal gameManager, non toccare.
*/

public class MovementManager {
    public Mappa map = GameManager.mapInstance;
    public Giocatore player = GameManager.playerInstance;
    public MovePlayer playerPrefabMover;

    public MovementManager()
    {
        // Ottieni lo script del prefab frontend
        playerPrefabMover = GameManager.playerPrefabInstance.GetComponent<MovePlayer>();
        // Centra il player
        //Debug.Log(map.dim / 2);
        player.move((map.dim / 2), (map.dim / 2));
    }

    // Ottiene in modo circolare la prossima tile partendo da x,y in direzione dir

    private MapTile getNextTile(int x, int y, Direction dir)
    {
        if (dir == Direction.nord)
        {
            y = (y + 1) % map.dim;
        }
        else if(dir == Direction.sud)
        {
            y = (y - 1) % map.dim;
        }
        else if (dir == Direction.est)
        {
            x = (x + 1) % map.dim;
        }
        else if (dir == Direction.ovest)
        {
            x = (x - 1) % map.dim;
        }

        if (y == -1) y = map.dim-1;
        if (x == -1) x = map.dim-1;

        //Debug.Log(x + " " + y);
        return map.getTile(x,y);
    }

    // Controlla se il giocatore si può muovere in direzione dir

    public bool canMove(Direction dir)
    {
        //Debug.Log("CONTROLLO SE PUO MUOVERSI IN " + dir);
        MapTile playerTile = map.getTile(player.getX(), player.getY());
        //Debug.Log("Il player si trova su un " + playerTile.getTileType() + " " + playerTile.getTileRotation());
        MapTile nextTile = getNextTile(player.getX(), player.getY(), dir);
        //Debug.Log("E vuole andare su un " + nextTile.getTileType() + " " + nextTile.getTileRotation());

        // Se la strada nel tile attuale è sbarrata
        //if (!playerTile.getDirection(dir)) Debug.Log("Il playertile è chiuso a " + dir);
        if (!playerTile.getDirection(dir)) return false;

        // Controlla la strada nel tile di destinazione
        if (dir == Direction.nord)
        {
            if (!nextTile.getDirection(Direction.sud))
            {
                //Debug.Log("Il nextile è chiuso a sud");
                return false;
            }
        }
        else if (dir == Direction.sud)
        {
            if (!nextTile.getDirection(Direction.nord))
            {
                //Debug.Log("Il nextile è chiuso a nord");
                return false;
            }
        }
        else if (dir == Direction.est)
        {
            if (!nextTile.getDirection(Direction.ovest))
            {
                //Debug.Log("Il nextile è chiuso a ovest");
                return false;
            }
        }
        else if (dir == Direction.ovest)
        {
            if (!nextTile.getDirection(Direction.est))
            {
                //Debug.Log("Il nextile è chiuso a est");
                return false;
            }
        }

        // Se ha passato tutti i controlli allora può muovere!
        //Debug.Log("Può muoversi!");
        return true;
    }

    // Muove effettivamente il giocatore

    public void movePlayer(Direction dir)
    {
        if (GameManager.moving || !TileMovement.canRot || !PlayerOnTileMovement.canRot) return;

        int x = player.getX();
        int y = player.getY();

        if (canMove(dir))
        {
            if (dir == Direction.nord)
            {
                y = (y + 1) % map.dim;
            }
            else if (dir == Direction.sud)
            {
                y = (y - 1) % map.dim;
            }
            else if (dir == Direction.est)
            {
                x = (x + 1) % map.dim;
            }
            else if (dir == Direction.ovest)
            {
                x = (x - 1) % map.dim;
            }

            if (y == -1) y = map.dim-1;
            if (x == -1) x = map.dim-1;
            //Debug.Log(x + " " + y);

            // Back end
            player.move(x, y);
            // Front end
            playerPrefabMover.move(x, y, false);

            // Scopri i tile
            getNextTile(x, y, Direction.nord).setFog(false);
            getNextTile(x, y, Direction.sud).setFog(false);
            getNextTile(x, y, Direction.est).setFog(false);
            getNextTile(x, y, Direction.ovest).setFog(false);

            // Se il player è sul bordo muovi il tile opposto solo in front end
            if (x == 0 || x == map.dim-1 || y == 0 || y == map.dim-1)
            {
                map.getTile(x, y).getPrefab().GetComponent<MoveWarpTiles>().WarpToMe();
            }

            GameManager.turno = Turno.strega;
            GameManager.witchInstance.Move();
        }

        return;
    }
}
