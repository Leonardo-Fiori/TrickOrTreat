using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // per event data 
public class MenuUI : MonoBehaviour{ /*,IPointerEnterHandler,*/ //IPointerExitHandler { //per il pointer enter e exit

    RectTransform rt;
    Vector3[] angles;
    Vector3 startPos;
    PointerEventData eventData;
    public bool hasBeenClicked = false;
    // Use this for initialization

    void Start () {
        rt = GetComponent<RectTransform>(); // transform per la ui
        angles = new Vector3[4];    // array con i 4 angoli di riferimento
        rt.GetLocalCorners(angles); // riempio l'array con i 4 angoli 
        startPos = rt.anchoredPosition; // mi salvo la posizione centrale di partenza così da ricentrarlo per bene una volta spostatosi
    }

    void Update()
    {
        if(hasBeenClicked) rt.anchoredPosition = angles[1];
        else if (!hasBeenClicked) rt.anchoredPosition = startPos;
    }

    public void ChangeBool()
    {
        print("Booleana cambiata, hasBeenClicked is " + hasBeenClicked);
        if (hasBeenClicked) hasBeenClicked = false;
        else if (!hasBeenClicked) hasBeenClicked = true;
    }
    /*public void OnPointerEnter(PointerEventData eventData)
    {
         
        //print("Hello " + eventData.pointerEnter.name);
        
        
        //print("Local corners are ");
        //for (int i = 0; i < 4; i++)
        //{
        //    print("Gli angoli sono " + i + ": " + angles[i]);
        //}

        rt.anchoredPosition = angles[1];    // posizione angolo del pannello da usare come nuovo centro

    }*/
    /*public void OnPointerExit(PointerEventData eventData)
    {
        rt.anchoredPosition = startPos;
        
    }*/


}
