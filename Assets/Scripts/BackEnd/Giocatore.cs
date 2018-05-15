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
    public int incrementoMosse = 0;

    private GameObject frontEndPrefab;
    private MovePlayer playerMover;

    public void Reset()
    {
        mosseFatte = 0;
        chiaviRaccolte = 0;
        caramelleRaccolte = 0;
        incrementoMosse = 0;
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
            GameManager.turno = Turno.strega;
            mosseFatte = 0;

            // uso il prefab per chiamare il movimento nel back end perchè essendo un mono beahviour ha la invoke!
            // InvokeMovement -> Move backend -> Move -> InvokeMovement
            GameManager.cameraManagerInstance.SwitchSubject();

            GameManager.witchPrefabInstance.GetComponent<MoveWitch>().InvokeMovement(.8f);
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
        x = newX;
        y = newY;

        // Front end
        playerMover.move(x, y, mov);

        if (GameManager.mapInstance.getTile(x, y).IsUscita() && chiaviRaccolte >= GameManager.mapInstance.GetQuanteChiavi())
        {
            GameManager.instance.Restart();
            return;
        }
    }
}
