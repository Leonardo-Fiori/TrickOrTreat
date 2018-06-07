using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableSaveData {

    public SerializableSaveData(int easy, int normal, int hard, int quality)
    {
        this.easy = easy;
        this.normal = normal;
        this.hard = hard;
        this.cameraQuality = quality;
    }

    public int easy;
    public int normal;
    public int hard;

    public int cameraQuality;

}
