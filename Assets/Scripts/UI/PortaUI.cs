using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PortaUI : MonoBehaviour {

    
    public Image portaChiusa;
    public Image portaAperta;


    private void Start()
    {
        portaChiusa.gameObject.SetActive(true);   
        portaAperta.gameObject.SetActive(false);

    }


    // L'EVENTO CHIAVE PRESA VIENE CHIAMATO PRIMA CHE LA CHIAVE VENGA EFFETTIVAMENTE CONTEGGIATA, PER QUESTO LA CONDIZIONE è CHIAVI +1
    public void CheckPortaAperta()
    {
        //print("Check porta");
        int chiavi = GameManager.playerInstance.GetChiavi();
        int chiaviMassime = GameManager.playerInstance.GetChiaviNecessarie();
        //print("Ho " + chiavi + " me ne servono " + chiaviMassime + " per entrare");
        if (chiaviMassime == chiavi+1)
        {
            portaChiusa.gameObject.SetActive(false);
            portaAperta.gameObject.SetActive(true);
            //print("switch immagini porta");
        }
    }
}
