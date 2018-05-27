using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

/* Mantiene i dati pubblici e privati del giocatore,
 * è instanziabile come scriptable obj dall'editor, deve essere passato al gameManager per far iniziare la partita.
 * dal riferimento statico nel gamemanager sono accessibili le sue funzioni getter e setter.
*/

[CreateAssetMenu(fileName = "Giocatore", menuName = "StatGiocatore", order = 2)]
public class Giocatore : ScriptableObject {
    private int x;
    private int y;

    private int mosseFatte;
    private int chiaviRaccolte;
    private int caramelleRaccolte;

    [SerializeField] private int mossePerTurno;
    [SerializeField] private int caramelleNecessarie;
    [HideInInspector] public int incrementoMosse = 0;

    private GameObject frontEndPrefab;
    private MovePlayer playerMover;

    public void Reset()
    {
        mosseFatte = 0;
        chiaviRaccolte = 0;
        caramelleRaccolte = 0;
        incrementoMosse = 0;
    }

    public void RaccogliScarpetta()
    {
        Debug.Log("Hai raccolto una scarpetta. Ti restano " + (mossePerTurno - mosseFatte) + " mosse.");

        mosseFatte--;
    }

    public void RaccogliCaramella()
    {
        caramelleRaccolte++;

        if (caramelleRaccolte >= caramelleNecessarie)
        {
            incrementoMosse++;
            Debug.Log("Mosse per turno incrementate: " + mossePerTurno + " " + incrementoMosse);
            caramelleRaccolte = 0;
        }
    }

    public void SetFrontEndPrefab(GameObject prefab)
    {
        Debug.Log(chiaviRaccolte);
        frontEndPrefab = prefab;
        playerMover = prefab.GetComponent<MovePlayer>();
    }

    public void ResetMosseFatte()
    {
        mosseFatte = 0;
    }

    public void ResetChiavi()
    {
        chiaviRaccolte = 0;
    }

    public void IncrementaChiavi()
    {
        chiaviRaccolte++;
    }

    public int GetChiavi()
    {
        return chiaviRaccolte;
    }

    public void IncrementaMosseFatte()
    {
        mosseFatte++;
        if (mosseFatte >= mossePerTurno + incrementoMosse)
        {
            mosseFatte = 0;

            GameManager.instance.SwitchTurn();
        }
    }

    public int GetMosseFatte()
    {
        return mosseFatte;
    }

    public int GetMossePerTurno()
    {
        return mossePerTurno + incrementoMosse;
    }

    public int GetCaramelleNecessarie()
    {
        return caramelleNecessarie;
    }

    public int GetChiaviNecessarie()
    {
        return GameManager.mapInstance.GetQuanteChiavi();
    }

    public int getX()
    {
        return x;
    }

    public int getY()
    {
        return y;
    }

    public void move(int newX, int newY, Movement mov)
    {
        GameManager.playerMovementEvent.Raise();

        x = newX;
        y = newY;

        if(mov == Movement.smooth)
            IncrementaMosseFatte();

        playerMover.move(x, y, mov);

        MapTile tile = GameManager.mapInstance.getTile(x, y);

        if (tile.HasCaramella())
        {
            RaccogliCaramella();
            tile.SetCaramella(false);
        }

        if (tile.HasKey())
        {
            IncrementaChiavi();
            tile.SetKey(false);
        }

        if (tile.HasScarpetta())
        {
            RaccogliScarpetta();
            tile.SetScarpetta(false);
        }

        if (GameManager.mapInstance.getTile(x, y).IsUscita() && chiaviRaccolte >= GameManager.mapInstance.GetQuanteChiavi())
        {
            GameManager.instance.Restart();
            return;
        }
    }
}
