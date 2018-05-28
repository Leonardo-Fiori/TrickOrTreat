using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrendiScarpetta : MonoBehaviour {
    public void Prendi()
    {
        GameManager.playerInstance.RaccogliScarpetta();
    }
}
