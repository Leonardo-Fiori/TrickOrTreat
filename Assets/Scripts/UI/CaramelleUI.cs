using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CaramelleUI : MonoBehaviour {

    public Image caramellaAttiva;
    public Image caramellaDisattiva;
    public static Image[] images;

    
    int j = 0; // lo uso per scorrere gli eventi per aggiungere caramelle
    


	// iniziallizzo un array di immagini e spawno nel pannello le caramelle disattivate
	void Start () {
        
        if (caramellaAttiva.tag != "CaramellaAttiva") throw new System.ArgumentException("Tag di " + caramellaAttiva + " non può essere nullo! Impostare nel prefab tag 'CaramellaAttiva' ");

        images = new Image[GameManager.playerInstance.GetCaramelleNecessarie()]; 

        for (int i = 0; i < images.Length; i++)
        {
            images[i] = Instantiate(caramellaDisattiva, transform);
            
        }
	}
	



    // aggiunge una sprite di caramella attiva distruggendo la sprite caramella spenta
    public void AddCaramella()
    {
        //print("Caramella added");

        images[j].gameObject.SetActive(false);  // disattivo prima di distruggere l'mmagine della caramella disattivata
        GameObject.Destroy(images[j].gameObject);   // distruggo la sprite

        images[j] = Instantiate(caramellaAttiva , transform);   // la sostituisco con la caramella attiva        

        j++;    // scorro per essere pronto all'evento successivo di presa caramella
    }


    // ad ogni presa di caramella controlla se l'array è pieno di caramelle attive, in tal caso resetta la ui 
    public void CheckResetCaramelleUI()
    {
        int controllo = 0;

        for (int i = 0; i < images.Length; i++)
        {
            if (images[i].CompareTag("CaramellaAttiva")) controllo++;            
            
        }
        print("controllo vale " + controllo);
        if (controllo == images.Length) ResetUiCaramelle();
    }


    // resetta le caramelle tutte disattivate ( instanziando le sprite delle caramelle disattivate)
    public void ResetUiCaramelle()
    {
        print("Disattivo le caramelle ");

        // se non si resetta forse è perchè caramelleraccolte non è mai uguale a caramelle necessarie
        for (int i = 0; i < images.Length; i++)
        {
            images[i].gameObject.SetActive(false);
            GameObject.Destroy( images[i].gameObject);
            images[i] = Instantiate(caramellaDisattiva, transform);
        }

        j = 0;
        
    }
}
