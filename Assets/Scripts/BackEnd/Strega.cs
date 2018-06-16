using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Strega", menuName = "Strega", order = 3)]
public class Strega : ScriptableObject {

    [HideInInspector]
    public int x;
    [HideInInspector]
    public int y;
    [HideInInspector]
    public int mosseFatte;
    [HideInInspector]
    public float chanceMossaRandom;
    [HideInInspector]
    public float chanceMossaRandomAttuale;
    [HideInInspector]
    public int id;

    public int mossePerTurno;

    private GameObject frontEndPrefab;
    private MoveWitch witchMover;

    private bool petardo = false;

    public WitchBrain cervello;

    // Serve per l'ai, non toccare
    private bool[,] visitedTiles;

    /* Funzioni Get() e Set() */

    public void ResetPetardo()
    {
        petardo = false;
    }

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
        chanceMossaRandomAttuale = chanceMossaRandom;

        // da rivedere la posizione iniziale!
        x = GameManager.mapInstance.getUscita(id)[0];
        y = GameManager.mapInstance.getUscita(id)[1];

        // Aggiorno front end
        witchMover.Move(x, y, Movement.teleport);

        int dim = GameManager.mapInstance.dimensione;
        visitedTiles = new bool[dim,dim];
        for (int i = 0; i < dim; i++)
            for (int j = 0; j < dim; j++)
                visitedTiles[i, j] = false;
    }

    private void Win()
    {
        Debug.Log("ciaoo");
        Giocatore.morto = true;
        GameManager.instance.eventoMorteGiocatore.Raise();
        //SoundManager.instance.Play("sconfitta");
        GameManager.instance.Restart();
        return;
    }

    public bool PlayerOnNextTile()
    {
        int playerX = GameManager.playerInstance.getX();
        int playerY = GameManager.playerInstance.getY();

        MapTile nord = GameManager.movementManagerInstance.getNextTile(x, y, Direction.nord);
        MapTile sud = GameManager.movementManagerInstance.getNextTile(x, y, Direction.sud);
        MapTile est = GameManager.movementManagerInstance.getNextTile(x, y, Direction.est);
        MapTile ovest = GameManager.movementManagerInstance.getNextTile(x, y, Direction.ovest);

        bool playerNord = playerX == nord.getX() && playerY == nord.getY();
        bool playerSud = playerX == sud.getX() && playerY == sud.getY();
        bool playerEst = playerX == est.getX() && playerY == est.getY();
        bool playerOvest = playerX == ovest.getX() && playerY == ovest.getY();

        return playerNord || playerSud || playerEst || playerOvest;
    }

    /* Chiamo la move per muovere la strega, gestisce l'AI */

    public void Move()
    {
        // Salta la mossa per il petardo
        if (petardo)
        {
            petardo = false;

            Debug.Log("la strega salta una mossa");

            mosseFatte++;

            if (mosseFatte >= mossePerTurno)
            {
                mosseFatte = 0;

                GameManager.instance.SwitchTurn();
            }

            return;
        }

        // Salta la mossa per la cheatmode
        if (GameManager.cheatMode)
        {
            mosseFatte = 0;

            GameManager.instance.SwitchTurn();

            return;
        }

        // Check win
        int playerX = GameManager.playerInstance.getX();
        int playerY = GameManager.playerInstance.getY();
        if (x == playerX && y == playerY)
        {
            Win();
            return;
        }

        // Fa la mossa
        cervello.Think(this);
        mosseFatte++;

        // Play suoni
        //SoundManager.instance.Play("witchstep");
        //SoundManager.instance.Play("whoosh");

        // Attiva il petardo se presente
        if (GameManager.mapInstance.getTile(x, y).IsPetardoAttivo())
        {
            petardo = true;
            GameManager.mapInstance.getTile(x, y).ScoppiaPetardo();
        }

        // Switch turn
        if (mosseFatte >= mossePerTurno)
        {
            mosseFatte = 0;

            GameManager.instance.SwitchTurn();
        }

        // Muovi il front end
        witchMover.Move(x, y, Movement.smooth);

        // Check win
        if (x == playerX && y == playerY)
        {
            Win();
            return;
        }

        // Evento movimento
        GameManager.instance.eventoMovimentoStrega.Raise();
    }
}
