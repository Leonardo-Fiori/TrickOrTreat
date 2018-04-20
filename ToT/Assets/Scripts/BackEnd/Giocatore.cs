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

    private int mosseDisponibili;
    private int chiaviRaccolte;
    private bool powerup;
    private int caramelleRaccolte;

    public int mossePerTurno;
    public int caramelleNecessarie;
    public int chiaviNecessarie;

    public int getX()
    {
        return x;
    }

    public int getY()
    {
        return y;
    }

    public void move(int newX, int newY)
    {
        x = newX;
        y = newY;
    }
}
