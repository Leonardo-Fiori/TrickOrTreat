using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListTurniUI : MonoBehaviour {

    public List<Image> images;    
    public Image turnoAttivo;
    public Image turnoDisattivato;
    public Image scarpetta;

    int j = 1;
    // Use this for initialization
    void Start () {

        //images = new List<Image>(new Image[GameManager.playerInstance.GetMossePerTurno()]);


        /*for (int i = 0; i < images.Count; i++)
        {
            images[i] = Instantiate(turnoAttivo, transform);
        }*/

        ////////////////////////////////////////////////////////////////////////////////
        images = new List<Image>(new Image[9]);
        for (int i = 0; i < GameManager.playerInstance.GetMossePerTurno(); i++) // da 0 a 2  attivo le mosse disponibili
        {
            images[i] = Instantiate(turnoAttivo, transform); // attivo le prime 3                   
        }

        for (int i = GameManager.playerInstance.GetMossePerTurno(); i < images.Count; i++) // da 3 a 5 metto quelle non disponibili
        {
            images[i] = Instantiate(turnoDisattivato, transform);   // disattivo quelle dopo
        }

        //print(images.Count);
        //print(GameManager.playerInstance.GetMossePerTurno());

    }
	

    public void ScalaTurno()
    {
        int turnoMassimo = GameManager.playerInstance.GetMossePerTurno();
        
       // print("ScalaTurno() in posizione " + (turnoMassimo - j));   // faccio la sottrazione a causa di come viene tenuto il conteggio dei turni che non conterebbe lo 0

        images[turnoMassimo - j].gameObject.SetActive(false);
        GameObject.Destroy(images[turnoMassimo - j]);

        images[turnoMassimo - j] = Instantiate(turnoDisattivato, transform);
        j++;

    }
    
    
    public void ScarpettaTaken()
    {
        int turnoMassimo = GameManager.playerInstance.GetMossePerTurno();
        if(j <= turnoMassimo) j--;  // check per evitare il reference expection, però la ui lo seguirà male

        /*print("Devo instanziare un turno attivo in posizione " + (turnoMassimo - j ));
        GameObject.Destroy(images[turnoMassimo - j]);
        images[turnoMassimo - j] = Instantiate(turnoAttivo);*/
    }
    public void TurnoPermanente()
    {
        int turnoMassimo = GameManager.playerInstance.GetMossePerTurno();

        //print("Mossa permanente");

        images.Insert(turnoMassimo - j , Instantiate(turnoAttivo, transform));
        
    }




    public void ResetTurno()
    {
       // print("ResetTurno() invocato, Resetto UI e conteggio ScalaTurno()");

        int turnoMassimo = GameManager.playerInstance.GetMossePerTurno();

        //if (turnoMassimo == images.Count) print("La capacità delle mosse massime è arrivata al massimo");

        for (int i = 0; i < turnoMassimo; i++) // 
        {
            images[i].gameObject.SetActive(false);
            GameObject.Destroy(images[i]);
            images[i] = Instantiate(turnoAttivo, transform); // attivo le prime 3                   
        }



        for (int i = turnoMassimo; i < images.Count; i++) // da 3 a 5 metto quelle non disponibili
        {
            images[i].gameObject.SetActive(false);
            GameObject.Destroy(images[i]);
            images[i] = Instantiate(turnoDisattivato, transform);   // disattivo quelle dopo
        }

        j = 1;
    }
}
