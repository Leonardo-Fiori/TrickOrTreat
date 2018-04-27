using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/* Mantiene i dati pubblici e privati del giocatore,
 * è instanziabile come scriptable obj dall'editor, deve essere passato al gameManager per far iniziare la partita.
 * dal riferimento statico nel gamemanager sono accessibili le sue funzioni getter e setter.
*/

[CreateAssetMenu(fileName = "Giocatore", menuName = "StatGiocatore", order = 2)]
public class Giocatore : ScriptableObject {
    private int x;
    private int y;

    private int mosseFatte;
    [HideInInspector] public int chiaviRaccolte;
    [HideInInspector] public bool powerup;
    [HideInInspector] public int caramelleRaccolte;

    [SerializeField] private int mossePerTurno;
    [SerializeField] private int caramelleNecessarie;
    [SerializeField] private int chiaviNecessarie;

    private GameObject frontEndPrefab;
    private MovePlayer playerMover;

    public void SetFrontEndPrefab(GameObject prefab)
    {
        frontEndPrefab = prefab;
        playerMover = prefab.GetComponent<MovePlayer>();
    }

    public void ResetMosseFatte()
    {
        mosseFatte = 0;
    }

    public void IncrementaMosseFatte()
    {
        mosseFatte++;
        if (mosseFatte >= mossePerTurno)
        {
            GameManager.turno = Turno.strega;
            mosseFatte = 0;

            // uso il prefab per chiamare il movimento nel back end perchè essendo un mono beahviour ha la invoke!
            // InvokeMovement -> Move backend -> Move -> InvokeMovement
            GameManager.witchPrefabInstance.GetComponent<MoveWitch>().InvokeMovement(0.5f);
        }
    }

    public int GetMosseFatte()
    {
        return mosseFatte;
    }

    public int GetMossePerTurno()
    {
        return mossePerTurno;
    }

    public int GetCaramelleNecessarie()
    {
        return caramelleNecessarie;
    }

    public int GetChiaviNecessarie()
    {
        return chiaviNecessarie;
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
    }
}
