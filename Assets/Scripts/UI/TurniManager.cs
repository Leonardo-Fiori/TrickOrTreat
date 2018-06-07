using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TurniManager : MonoBehaviour
{
    int mosseMassime = 8;    // Gamemanger.difficulty.value caramelle dal tileset e numero di caramelle necessarie dal player
    
    public Button skipTurn;
    public GameObject panel;
    public Image turnoAttivo;
    public Image turnoDisattivato;
    public Image scarpetta;
    public Image[] arrayImmagini;
    
    public Giocatore giocatore;

    int posizioneScarpetta;    // ultima posizione pannello immagini riservata alla scarpetta

    int j = 1; // la uso per scorrermi i turni


    // creo un array con le mosse massime che il bambino potrà mai fare
    void Start()
    {
        print(GameManager.instance.difficulty.value);
        arrayImmagini = new Image[mosseMassime];        // iniziallizzo l'array con le immagini
        posizioneScarpetta = arrayImmagini.Length - 1;

        SetUI();

        //print("Hello ui");
    }

    // Update is called once per frame
    void Update()
    {
        int turnoMax = GameManager.playerInstance.GetMossePerTurno();
        int turno = GameManager.playerInstance.GetMosseFatte();
        //print("J vale " + j);
        //print("Il turnoMAX" + turnoMax);
        //print("Il turno attuale è " + turno);     
              
    }
    
    
    // setta la ui con le mossime massime disponibili da quel turno
    public void SetUI()    
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



    // Resetta la j a 1 ascoltando la strega che si muove
    public void ResetTurno()
    {
        //print("ResetTurno() invocato, Resetto UI e conteggio ScalaTurno()");

        int turnoMassimo = GameManager.playerInstance.GetMossePerTurno();
        if (turnoMassimo == arrayImmagini.Length) print("La capacità delle mosse massime è arrivata al massimo");

        for (int i = 0; i < turnoMassimo ; i++) // 
        {
            arrayImmagini[i].gameObject.SetActive(false);
            GameObject.Destroy(arrayImmagini[i]);
            arrayImmagini[i] = Instantiate(turnoAttivo, panel.transform); // attivo le prime 3                   
        }

        

        for (int i = turnoMassimo; i < arrayImmagini.Length; i++) // da 3 a 5 metto quelle non disponibili
        {
            arrayImmagini[i].gameObject.SetActive(false);
            GameObject.Destroy(arrayImmagini[i]);
            arrayImmagini[i] = Instantiate(turnoDisattivato, panel.transform);   // disattivo quelle dopo
        }

        j = 1;
    }


    // evento che ascolta la presa della scarpetta, la j che sarà aumentata nello scalaturno della presa della scarpetta dovrà essere riportata al valore precedente
    public void ScarpettaTaken()
    {
        print("Scarpetta presa j vale " + j);
        arrayImmagini[posizioneScarpetta].gameObject.SetActive(false);
        GameObject.Destroy(arrayImmagini[posizioneScarpetta]);
        arrayImmagini[posizioneScarpetta] = Instantiate(scarpetta, transform);
        /*int turnoMassimo = GameManager.playerInstance.GetMossePerTurno();

        j--;

        arrayImmagini[turnoMassimo - j].gameObject.SetActive(false);
        GameObject.Destroy(arrayImmagini[turnoMassimo - j]);

        arrayImmagini[turnoMassimo - j] = Instantiate(turnoAttivo, transform);
        */
    }


    // è quello che sostituisce la sprite di turno bloccato 
    public void ScalaTurno()
    {
        int turnoMassimo = GameManager.playerInstance.GetMossePerTurno();
       // print("ScalaTurno() in posizione " + (turnoMassimo - j));   // faccio la sottrazione a causa di come viene tenuto il conteggio dei turni che non conterebbe lo 0

        arrayImmagini[turnoMassimo - j].gameObject.SetActive(false);
        GameObject.Destroy(arrayImmagini[turnoMassimo - j]);

        arrayImmagini[turnoMassimo - j] = Instantiate(turnoDisattivato, transform);
        j++;

       
    }


    // QUESTA DEVE GARANTIRE UN TURNO IN PIU' ALLA PRESA DELLE 3 CARAMELLE
    
    public void TurnoPermanente()
    {
        // CHECK sul numero di caramelle che ho vs le caramelle necessarie per aumentare le mosse permanentemente
    }

    // funzione del pulsante per skippare il turno
    public void PassaTurno()
    {
        //print("Turno passato");
        if (GameManager.turno == Turno.giocatore) GameManager.instance.SwitchTurn();
       
    }
}