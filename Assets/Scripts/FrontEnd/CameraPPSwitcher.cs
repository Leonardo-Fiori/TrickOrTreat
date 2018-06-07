using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class CameraPPSwitcher : MonoBehaviour {

    public SOCameraPPValue qualityLevel;
    private int quality = 0;
	
	void Update () {
        if (GameManager.debugMode)
        {
            if (Input.GetKeyUp(KeyCode.F1))
            {
                qualityLevel.Decrease();
                print("Quality level: " + qualityLevel.value);
            }
            else if (Input.GetKeyUp(KeyCode.F2))
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
