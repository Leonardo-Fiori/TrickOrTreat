using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttivaPetardo : MonoBehaviour
{

    //Giocatore player;
    public static bool toggle;
    //public GameObject fantoccio;

    private void Start()
    {
        //player = GameManager.playerInstance;
        toggle = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if ((GameManager.playerInstance.HasPetardo() || GameManager.debugMode))
            {
                toggle = !toggle;

                print("Modalità petardo: " + toggle);

                if (toggle)
                    SoundManager.instance.Play("pickpetardo", 0.5f);
                else
                    SoundManager.instance.Play("clicksimple", 0.5f);
            }
        }
    }
}
