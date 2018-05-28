using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PortaUI : MonoBehaviour {

    public Image[] images;

    private void Start()
    {
        images[1].gameObject.SetActive(true);   // attivo sprite porta disabled
        images[0].gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update () {

        int chiavi = GameManager.playerInstance.GetChiavi();
        int chiaviMassime = GameManager.playerInstance.GetChiaviNecessarie();
        //print("Ho chiavi = " + chiavi);
        //print("chiavi massime " + chiaviMassime);
        // check se sprite porta aperta
        if (chiaviMassime <= chiavi)
        {
            images[1].gameObject.SetActive(false);  // disattivo sprite porta disabled
            images[0].gameObject.SetActive(true);   //  attivo sprite porta enabled
            //print("Ho le chiavi");
        }
        // else porta chiusa
        else
        {
            //print("Mancano le chiavi");
        }
    }
}
