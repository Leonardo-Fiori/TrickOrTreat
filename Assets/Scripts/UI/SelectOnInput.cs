using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class SelectOnInput : MonoBehaviour
{

    // QUESTO SCRIPT SERVE A DETECTARE NEL MENU SE VIENE USATO KEYBOARD O GAMEPAD
    public EventSystem eventSystem;
    public GameObject selectedObject;   //il primo oggetto che selezionerà

    private bool buttonSelected = false;

    // Update is called once per frame
    void Update()
    {

        if ((Input.GetAxisRaw("Vertical")) != 0 && buttonSelected == false)
        {  // se detecto qualsiasi movimento sul vertical axis da keyboard o gamepad(frecce su\giu e WS?)

            eventSystem.SetSelectedGameObject(selectedObject);
            buttonSelected = true;
        }
    }

    private void OnDisable()
    {
        buttonSelected = false;     //?
    }
}