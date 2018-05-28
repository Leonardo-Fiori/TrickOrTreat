using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CaramelleUI : MonoBehaviour {
    public Image caramellaAttiva;
    public Image[] images;
     
	// Use this for initialization
	void Start () {
        
        images = new Image[GameManager.playerInstance.GetCaramelleNecessarie()]; // 5

        for (int i = 0; i < images.Length; i++)
        {
            images[i] = Instantiate(caramellaAttiva, transform);
            images[i].gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
        print("Hai " + GameManager.playerInstance.caramelleRaccolte + " al momento"); // arriva massimo a 4

        images[GameManager.playerInstance.caramelleRaccolte].gameObject.SetActive(true);
        //for (int i = 0; i < images.Length)
	}
}
