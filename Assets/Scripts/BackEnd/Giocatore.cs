using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

/* Mantiene i dati pubblici e privati del giocatore,
 * è instanziabile come scriptable obj dall'editor, deve essere passato al gameManager per far iniziare la partita.
 * dal riferimento statico nel gamemanager sono accessibili le sue funzioni getter e setter.
*/

[CreateAssetMenu(fileName = "Giocatore", menuName = "Giocatore", order = 2)]
public class Giocatore : ScriptableObject {
    private int x;
    private int y;

    public static bool morto;

    private int mosseFatte;
    private int chiaviRaccolte;
    private int caramelleRaccolte;

    private bool petardo = false;

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
        petardo = false;
    }

    public void RaccogliScarpetta(MapTile tile)
    {
        //SoundManager.instance.Play("pickscarpetta");

        tile.PrendiScarpetta();

        GameManager.instance.eventoScarpettaPresa.Raise();

        mosseFatte--;

        Debug.Log("Hai raccolto una scarpetta. Ti restano " + (mossePerTurno - mosseFatte) + " mosse.");
    }

    public void RaccogliPetardo(MapTile tile)
    {
        if (petardo)
            return;

        GameManager.instance.eventoPetardoPreso.Raise();

        //SoundManager.instance.Play("pickpetardo");

        tile.SetPetardo(false);

        petardo = true;
    }

    public void UsaPetardo()
    {
        GameManager.instance.eventoPetardoUsato.Raise();

        petardo = false;
    }

    public bool HasPetardo()
    {
        return petardo;
    }

    public void RaccogliCaramella(MapTile tile)
    {
        //SoundManager.instance.Play("pickcaramella");

        caramelleRaccolte++;

        tile.SetCaramella(false);

        GameManager.instance.eventoCaramellaPresa.Raise();

        if (caramelleRaccolte >= caramelleNecessarie)
        {
            incrementoMosse++;
            Debug.Log("Mosse per turno incrementate: " + mossePerTurno + " " + incrementoMosse);
            caramelleRaccolte = 0;
        }
    }

    public void SetFrontEndPrefab(GameObject prefab)
    {
        //Debug.Log(chiaviRaccolte);
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

    public void IncrementaChiavi(MapTile tile)
    {
        //SoundManager.instance.Play("pickchiave");

        tile.SetKey(false);

        GameManager.instance.eventoChiavePresa.Raise();

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
    
    public int GetCaramelleRaccolte()
    {
        return caramelleRaccolte;
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
        x = newX;
        y = newY;

        playerMover.move(x, y, mov);

        MapTile tile = GameManager.mapInstance.getTile(x, y);

        if (tile.HasCaramella())
        {
            RaccogliCaramella(tile);
        }

        if (tile.HasKey())
        {
            IncrementaChiavi(tile);
        }

        if (tile.HasScarpetta())
        {
            RaccogliScarpetta(tile);
        }

        if (tile.HasPetardo())
        {
            RaccogliPetardo(tile);
        }

        if (mov == Movement.smooth)
        {
            GameManager.instance.eventoMovimentoGiocatore.Raise();

            if(!tile.HasScarpetta())
                IncrementaMosseFatte();
        }

        if (GameManager.mapInstance.getTile(x, y).IsUscita() && chiaviRaccolte >= GameManager.mapInstance.GetQuanteChiavi())
        {
            GameManager.instance.eventoVittoriaGiocatore.Raise();
            //SoundManager.instance.Play("win");
            GameManager.instance.Restart();
            return;
        }
    }
}
