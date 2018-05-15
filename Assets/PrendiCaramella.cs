using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrendiCaramella : MonoBehaviour {
    public void Prendi()
    {
        GameManager.playerInstance.RaccogliCaramella();
    }
}
