using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PetardoUI : MonoBehaviour {

    public Image[] images;

    private void Start()
    {
        PetardoOff();
    }    

    public void PetardoOn()
    {
        images[0].gameObject.SetActive(false);
        images[1].gameObject.SetActive(true);
    }


    public void PetardoOff()
    {
        images[0].gameObject.SetActive(true);
        images[1].gameObject.SetActive(false);
    }
}
