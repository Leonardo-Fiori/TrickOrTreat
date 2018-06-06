using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CaramelleUI : MonoBehaviour {

    public Image caramellaAttiva;
    public Image caramellaDisattiva;
    public static Image[] images;

    //public SOEvent evento;
    int j = 0; // lo uso per scorrere gli eventi per aggiungere caramelle
    


	// iniziallizzo un array di immagini e spawno nel pannello le caramelle disattivate
	void Start () {
        
        images = new Image[GameManager.playerInstance.GetCaramelleNecessarie()]; 

        for (int i = 0; i < images.Length; i++)
        {
            images[i] = Instantiate(caramellaDisattiva, transform);
            
        }
	}
	


	// Update is called once per frame
	void Update () {

        //print("Caramelle che ho"+GameManager.playerInstance.GetCaramelleRaccolte());
        //print("Caramelle necessarie" + GameManager.playerInstance.GetCaramelleNecessarie());

        // if caramelle prese corrisponde a get caramelle necessarie disattivo tutte le caramelle e aggiungo una mossa duratura ai turni
        if (GameManager.playerInstance.GetCaramelleRaccolte() == GameManager.playerInstance.GetCaramelleNecessarie())
        {
            print("Resetto ui caramelle");
            ResetUiCaramelle();
            j = 0;  // lo riporto a 0 per ricontare da 0
        }
        //if (Input.GetKeyDown(KeyCode.M)) evento.Raise();    // per fargli partire l'evento della presa caramella
	}



    // aggiunge una sprite di caramella attiva distruggendo la sprite caramella spenta
    public void AddCaramella()
    {
        print("Caramella added");

        images[j].gameObject.SetActive(false);  // disattivo prima di distruggere l'mmagine della caramella disattivata
        GameObject.Destroy(images[j].gameObject);   // distruggo la sprite

        images[j] = Instantiate(caramellaAttiva , transform);   // la sostituisco con la caramella attiva        

        j++;    // scorro per essere pronto all'evento successivo di presa caramella
    }


    // resetta le caramelle tutte disattivate ( instanziando le sprite delle caramelle disattivate)
    public void ResetUiCaramelle()
    {
        // se non si resetta forse è perchè caramelleraccolte non è mai uguale a caramelle necessarie
        for (int i = 0; i < images.Length; i++)
        {
            images[i].gameObject.SetActive(false);
            GameObject.Destroy( images[i].gameObject);
            images[i] = Instantiate(caramellaDisattiva, transform);
        }
    }
}
