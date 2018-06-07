using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

[CreateAssetMenu(menuName = "Camera Quality Level Holder")]
public class SOCameraPPValue : ScriptableObject {
    public int value = 1;
    public List<PostProcessingProfile> settings;

    public void Increase()
    {
        value++;
        if (value > settings.Count - 1)
            value = settings.Count - 1;
    }

    public void Decrease()
    {
        value--;
        if (value < 0)
            value = 0;
    }
}
