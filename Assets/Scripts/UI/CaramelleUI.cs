using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CaramelleUI : MonoBehaviour {

    public Image caramellaAttiva;
    public Image[] images;
     
	// Use this for initialization
	void Start () {
        
        images = new Image[GameManager.playerInstance.GetCaramelleNecessarie()]; 

        for (int i = 0; i < images.Length; i++)
        {
            images[i] = Instantiate(caramellaAttiva, transform);
            images[i].gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {

       // print("Hai " + GameManager.playerInstance.GetCaramelleRaccolte() + " caramelle al momento"); // arriva massimo a caramelle necessarie -1

        images[GameManager.playerInstance.GetCaramelleRaccolte()].gameObject.SetActive(true);
        if(GameManager.playerInstance.GetCaramelleRaccolte() == (images.Length -1))
        {
            for (int i = 0; i< images.Length; i++)
            {
                images[i].gameObject.SetActive(false);
            }
        } 
        //for (int i = 0; i < images.Length)
	}
}
