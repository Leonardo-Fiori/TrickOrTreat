using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Strega", menuName = "Strega", order = 3)]
public class Strega : ScriptableObject {
    private int x;
    private int y;
    private int mosseDisponibili;

    public int mossePerTurno;

    private GameObject frontEndPrefab;
    private MoveWitch witchMover;

    /* Funzioni Get() e Set() */

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

    public int GetMosse()
    {
        return mosseDisponibili;
    }

    public void SetMosse(int quante)
    {
        mosseDisponibili = quante;
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
        x = 0;
        y = 0;

        // Aggiorno front end
        witchMover.Move(x, y);
    }

    /* Chiamo la move per muovere la strega, gestisce l'AI */

    public void Move()
    {
        // gestiscila come ti pare, basta che io possa chiamare questa funzione dal gamemanager o dal movementmanager

        // <-- INSERT YOUR CODE HERE DODOZ, SHOW ME YOUR SKILLZ <3

        witchMover.Move(x, y); // Alla fine, aggiorna il front end con questa chiamata
    }
}
