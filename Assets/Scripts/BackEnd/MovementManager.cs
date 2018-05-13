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

    private MoveWitch witchMover;

    public MovementManager()
    {
        //Debug.Log("movement manager initialized");
        witchMover = GameManager.witchPrefabInstance.GetComponent<MoveWitch>();
    }

    // Ottiene in modo circolare la prossima tile partendo da x,y in direzione dir

    public MapTile getNextTile(int x, int y, Direction dir)
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

    public List<MapTile> getNextTiles(int x, int y, Direction dir, int howMany)
    {
        List<MapTile> res = new List<MapTile>();

        for(int i = 0; i < howMany; i++)
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

            if (y == -1) y = map.dim - 1;
            if (x == -1) x = map.dim - 1;

            res.Add(map.getTile(x, y));
        }

        return res;
    }

    public int[] TileDistance(int x1, int y1, int x2, int y2)
    {
        int xDist = 0;
        int yDist = 0;
        int left = 0;
        int right = 0;
        int countLeft = 0;
        int countRight = 0;
        int up = 0;
        int down = 0;
        int countUp = 0;
        int countDown = 0;

        // Conta orizzontalmente

        right = x1;
        while(right != x2)
        {
            if (right == x2)
                break;

            right = (right + 1) % map.dim;

            if (right == -1) right = map.dim - 1;

            countRight++;
        }

        left = x1;
        while (left != x2)
        {
            if (left == x2)
                break;

            left = (left - 1) % map.dim;

            if (left == -1) left = map.dim - 1;

            countLeft++;
        }

        xDist = Mathf.Min(countLeft, countRight);

        // Conta verticalmente

        up = y1;
        while (up != y2)
        {
            if (up == y2)
                break;

            up = (up + 1) % map.dim;

            if (up == -1) up = map.dim - 1;

            countUp++;
        }

        down = y1;
        while (down != y2)
        {
            if (down == y2)
                break;

            down = (down - 1) % map.dim;

            if (down == -1) down = map.dim - 1;

            countDown++;
        }

        yDist = Mathf.Min(countDown, countUp);

        int[] res = { xDist, yDist };

        return res;
    }

    // Controlla se il giocatore si può muovere in direzione dir

    public bool canMove(Direction dir)
    {
        //Debug.Log("CONTROLLO SE PUO MUOVERSI IN " + dir);
        MapTile playerTile = map.getTile(player.getX(), player.getY());
        //Debug.Log("Il player si trova su un " + playerTile.getTileType() + " " + playerTile.getTileRotation());
        MapTile nextTile = getNextTile(player.getX(), player.getY(), dir);
        //Debug.Log("E vuole andare su un " + nextTile.getTileType() + " " + nextTile.getTileRotation());

        if(nextTile.getX() == GameManager.witchInstance.GetX() && nextTile.getY() == GameManager.witchInstance.GetY())
            return false;

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
        //Debug.Log(GameManager.turno+" "+TileMovement.canRot + " " + MoveWarpTiles.animating);
        if (GameManager.turno == Turno.strega) return;
        if (MovePlayer.moving || !TileMovement.canRot || MoveWarpTiles.animating) return;

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

            // Scopri i tile

            getNextTile(x, y, Direction.nord).setFog(false);
            getNextTile(x, y, Direction.sud).setFog(false);
            getNextTile(x, y, Direction.est).setFog(false);
            getNextTile(x, y, Direction.ovest).setFog(false);

            // Muovi il giocatore back end (muove da solo il front end)

            player.move(x, y, Movement.smooth);

            //if (!GameManager.cheatMode)
            player.IncrementaMosseFatte();

            GameManager.playerMovementEvent.Raise();

            // Se è il momento, switcha il turno

            /*if(player.GetMosseFatte() >= player.GetMossePerTurno()) // SPOSTATO NEL PLAYER BACK END
            {
                GameManager.turno = Turno.strega;
                player.ResetMosseFatte();
                //Debug.Log("Giocatore: turno della strega...");

                // uso il prefab per chiamare il movimento nel back end perchè essendo un mono beahviour ha la invoke!
                // InvokeMovement -> Move backend -> Move -> InvokeMovement
                GameManager.witchPrefabInstance.GetComponent<MoveWitch>().InvokeMovement(0.5f);
            }*/
        }

        return;
    }
}
