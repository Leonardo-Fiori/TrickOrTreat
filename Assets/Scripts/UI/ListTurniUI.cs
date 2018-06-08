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

        images = new List<Image>(new Image[GameManager.playerInstance.GetMossePerTurno()]);
                          

        for (int i = 0; i < images.Count; i++)
        {
            images[i] = Instantiate(turnoAttivo, transform);
        }

        print(images.Count);
        print(GameManager.playerInstance.GetMossePerTurno());

    }
	
	// Update is called once per frame
	void Update () {
        //if (Input.GetKeyDown(KeyCode.K)) TurnoPermanente();
        print(j);
	}

    public void ScalaTurno()
    {
        int turnoMassimo = GameManager.playerInstance.GetMossePerTurno();
        
        print("ScalaTurno() in posizione " + (turnoMassimo - j));   // faccio la sottrazione a causa di come viene tenuto il conteggio dei turni che non conterebbe lo 0

        images[turnoMassimo - j].gameObject.SetActive(false);
        GameObject.Destroy(images[turnoMassimo - j]);

        images[turnoMassimo - j] = Instantiate(turnoDisattivato, transform);
        j++;

    }

    //public void TurnoPermanente()
    //{
    //    print("Mossa permanente");
    //    images.Insert(jInstantiate(turnoAttivo , transform));
    //    images.
    //    j--;
    //}




    public void ResetTurno()
    {
        print("ResetTurno() invocato, Resetto UI e conteggio ScalaTurno()");

        int turnoMassimo = GameManager.playerInstance.GetMossePerTurno();

        if (turnoMassimo == images.Count) print("La capacità delle mosse massime è arrivata al massimo");

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
