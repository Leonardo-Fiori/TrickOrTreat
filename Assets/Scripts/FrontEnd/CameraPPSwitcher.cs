using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class CameraPPSwitcher : MonoBehaviour {

    public SOCameraPPValue qualityLevel;
    private int quality = 0;
    public bool needsDebugMode = false;
	
	void Update () {
        if (GameManager.debugMode || !needsDebugMode)
        {
            if (Input.GetKeyUp(GameManager.controls.decrementaGrafica))
            {
                qualityLevel.Decrease();
                print("Quality level: " + qualityLevel.value);
            }
            else if (Input.GetKeyUp(GameManager.controls.incrementaGrafica))
            {
                qualityLevel.Increase();
                print("Quality level: " + qualityLevel.value);
            }
        }

        if(quality != qualityLevel.value)
        {
            quality = qualityLevel.value;
            GetComponent<PostProcessingBehaviour>().profile = qualityLevel.settings[quality];
        }
	}
}
