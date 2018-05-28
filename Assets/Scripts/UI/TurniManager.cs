using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TurniManager : MonoBehaviour
{
    public int mosseMassime = 5;
    public Button skipTurn;
    public GameObject panel;
    public Image turnoAttivo;
    public Image turnoDisattivato;
    public Image[] arrayImmagini;

    public Giocatore giocatore;

    void Start()
    {
        arrayImmagini = new Image[mosseMassime];        // iniziallizzo l'array con le immagini


        SetUI();

        //print("Hello ui");
        // disegno le immagini su ui e le infilo in un array
        /*arrayImmagini = new Image[5];//[GameManager.playerInstance.GetMossePerTurno()];
        
        for (int i = /*GameManager.playerInstance.GetMossePerTurno()*//* 4; i >= 0; i--)    // lo faccio all'inverso perchè mossefatte partirà dalla posizione 0 
        {
            arrayImmagini[i] = TurniManager.Instantiate(turnoAttivo, panel.transform);
            arrayImmagini[i].gameObject.SetActive(false);

        }*/
    }

    // Update is called once per frame
    void Update()
    {
        int turnoMax = GameManager.playerInstance.GetMossePerTurno();
        int turno = GameManager.playerInstance.GetMosseFatte();

        print("Il turnoMAX" + turnoMax);
        print("Il turno attuale è " + turno);

        // se è il turno della strega la ui viene resettata
        if(GameManager.turno == Turno.strega)
        {
            ResetUI();
        }

        arrayImmagini[turno] = turnoDisattivato;
        /*for (int i = turnoMax - 1; i >= 0; i--)
        {
            arrayImmagini[i]
        }*/
        /*print(GameManager.playerInstance.GetMossePerTurno());
        // print("Le mosse fatte sono " + GameManager.playerInstance.GetMosseFatte()); //VA DA 0 A 2

        // se è il turno della strega la UI mi si ricarica (controintuitivo lo so lo so)
        if (GameManager.turno == Turno.strega)
        {
            for (int i = GameManager.playerInstance.GetMossePerTurno() - 1; i >= 0; i--)
            {
                //arrayImmagini[i].gameObject.SetActive(true);
                arrayImmagini[i].enabled = true;
            }
        }

        int turnoAttuale = GameManager.playerInstance.GetMosseFatte();

        //arrayImmagini[turnoAttuale].gameObject.SetActive(false);           
        arrayImmagini[turnoAttuale].enabled = false;*/
    }


    void SetUI()    // setta la ui all'inizio instanziando le immagini
    {
        for (int i = 0; i < GameManager.playerInstance.GetMossePerTurno(); i++) // da 0 a 2  attivo le mosse disponibili
        {
            arrayImmagini[i] = Instantiate(turnoAttivo, panel.transform); // attivo le prime 3                   
        }

        for (int i = GameManager.playerInstance.GetMossePerTurno(); i < arrayImmagini.Length; i++) // da 3 a 5 metto quelle non disponibili
        {
            arrayImmagini[i] = Instantiate(turnoDisattivato, panel.transform);   // disattivo quelle dopo
        }
    }

    void ResetUI()  // sostituisce nell'array le immagini 
    {
        for (int i = 0; i < GameManager.playerInstance.GetMossePerTurno(); i++) // da 0 a 2  attivo le mosse disponibili
        {
            arrayImmagini[i] = turnoAttivo;              
        }

        for (int i = GameManager.playerInstance.GetMossePerTurno(); i < arrayImmagini.Length; i++) // da 3 a 5 metto quelle non disponibili
        {
            arrayImmagini[i] = turnoDisattivato;
        }
    }

    void ScaleTurno(int turnoAttuale)
    {
        for (int i = GameManager.playerInstance.GetMossePerTurno()-1; i >= 0; i--)    // CONTROLLA QUESTO MAGGIORE O UGUALE A 0
        {
            arrayImmagini[i] = turnoDisattivato;
        }
    }

    public void PassaTurno()
    {
        if (GameManager.turno == Turno.giocatore)   // bisogna fare in modo che il turno switchi esattamente alla fine dei movimenti della strega
        {
            GameManager.turno = Turno.strega;

            GameManager.cameraManagerInstance.SwitchSubject();

            GameManager.witchPrefabInstance.GetComponent<MoveWitch>().InvokeMovement(.8f);

            GameManager.playerInstance.ResetMosseFatte();
        }
    }
}