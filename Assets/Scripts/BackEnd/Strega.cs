using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Strega", menuName = "Strega", order = 3)]
public class Strega : ScriptableObject {
    private int x;
    private int y;
    private int mosseFatte;
    public SOEvent eventoMovimento;

    [SerializeField] private int mossePerTurno;

    private GameObject frontEndPrefab;
    private MoveWitch witchMover;

    // Serve per l'ai, non toccare
    private bool[,] visitedTiles;

    /* Funzioni Get() e Set() */

    public void ResetMosseFatte()
    {
        mosseFatte = 0;
    }

    public int GetMossePerTurno()
    {
        return mossePerTurno;
    }

    public int GetMosseFatte()
    {
        return mosseFatte;
    }

    public int GetX()
    {
        return x;
    }

    public void SetX(int x)
    {
        this.x = x;
    }

    public int GetY()
    {
        return y;
    }

    public void SetY(int y)
    {
        this.y = y;
    }

    /* Funzione per settare il prefab rappresentante la strega nel front end */

    public void SetFrontEndPrefab(GameObject prefab)
    {
        frontEndPrefab = prefab;
        witchMover = frontEndPrefab.GetComponent<MoveWitch>();
    }


    /* Chiamo la Spawn() dal Game manager per muovere il prefab in posizione di avvio nel front end e inizializzare il back end */

    public void Spawn()
    {
        // da rivedere la posizione iniziale!
        x = GameManager.mapInstance.GetUscitaX();
        y = GameManager.mapInstance.GetUscitaY();

        // Aggiorno front end
        witchMover.Move(x, y, Movement.teleport);

        int dim = GameManager.mapInstance.dim;
        visitedTiles = new bool[dim,dim];
        for (int i = 0; i < dim; i++)
            for (int j = 0; j < dim; j++)
                visitedTiles[i, j] = false;
    }

    /* Chiamo la move per muovere la strega, gestisce l'AI */

    public void Move()
    {
        if (GameManager.cheatMode)
        {
            mosseFatte = 0;

            GameManager.instance.SwitchTurn();

            return;
        }

        int playerX = GameManager.playerInstance.getX();
        int playerY = GameManager.playerInstance.getY();

        if (x == playerX && y == playerY)
        {
            GameManager.instance.Restart();
            return;
        }

        MapTile nord = GameManager.movementManagerInstance.getNextTile(x, y, Direction.nord);
        MapTile sud = GameManager.movementManagerInstance.getNextTile(x, y, Direction.sud);
        MapTile ovest = GameManager.movementManagerInstance.getNextTile(x, y, Direction.ovest);
        MapTile est = GameManager.movementManagerInstance.getNextTile(x, y, Direction.est);

        int distanzaX = 1000; // what are you doing domi
        int distanzaY = 1000;
        int bestTileX = 0;
        int bestTileY = 0;

        if (((Mathf.Abs(nord.getX() - playerX)) <= distanzaX) && (Mathf.Abs(nord.getY() - playerY)) <= distanzaY)
        {
            bestTileX = nord.getX();
            bestTileY = nord.getY();
            distanzaX = Mathf.Abs(nord.getX() - playerX);
            distanzaY = Mathf.Abs(nord.getY() - playerY);
        }

        if (((Mathf.Abs(sud.getX() - playerX)) <= distanzaX) && (Mathf.Abs(sud.getY() - playerY) <= distanzaY))
        {
            bestTileX = sud.getX();
            bestTileY = sud.getY();
            distanzaX = Mathf.Abs(sud.getX() - playerX);
            distanzaY = Mathf.Abs(sud.getY() - playerY);
        }

        if (((Mathf.Abs(ovest.getX() - playerX)) <= distanzaX) && (Mathf.Abs(ovest.getY() - playerY) <= distanzaY))
        {
            bestTileX = ovest.getX();
            bestTileY = ovest.getY();
            distanzaX = Mathf.Abs(ovest.getX() - playerX);
            distanzaY = Mathf.Abs(ovest.getY() - playerY);
        }


        if (((Mathf.Abs(est.getX() - playerX)) <= distanzaX) && (Mathf.Abs(est.getY() - playerY)) <= distanzaY)
        {
            bestTileX = est.getX();
            bestTileY = est.getY();
            distanzaX = Mathf.Abs(est.getX() - playerX);
            distanzaY = Mathf.Abs(est.getY() - playerY);
        }

        x = bestTileX;
        y = bestTileY;

        int dim = GameManager.mapInstance.dim;

        mosseFatte++;

        witchMover.Move(x, y, Movement.smooth);

        if (x == playerX && y == playerY) 
        {
            GameManager.instance.Restart();
            return;
        }

        if (mosseFatte >= mossePerTurno)
        {
            mosseFatte = 0;

            GameManager.instance.SwitchTurn();
        }

        eventoMovimento.Raise();
    }

    /*
    private int CalcDistanceToPlayer(MapTile tile)
    {
        int playerX = GameManager.playerInstance.getX();
        int playerY = GameManager.playerInstance.getY();

        if (tile.getX() == playerX && tile.getY() == playerY)
            return 0;

        if (visitedTiles[tile.getX(), tile.getY()]) return 1;
        visitedTiles[tile.getX(), tile.getY()] = true;

        int distanceNord = CalcDistanceToPlayer(GameManager.movementManagerInstance.getNextTile(x, y, Direction.nord));
        int distanceSud = CalcDistanceToPlayer(GameManager.movementManagerInstance.getNextTile(x, y, Direction.sud));
        int distanceEst = CalcDistanceToPlayer(GameManager.movementManagerInstance.getNextTile(x, y, Direction.est));
        int distanceOvest = CalcDistanceToPlayer(GameManager.movementManagerInstance.getNextTile(x, y, Direction.ovest));

        return 1 + Mathf.Min(distanceNord, distanceSud, distanceEst, distanceOvest);
    }
    */
}
