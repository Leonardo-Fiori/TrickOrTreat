using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPlaySound : MonoBehaviour {

    public string soundName;
    public float volume;
    
    public void Play()
    {
        SoundManager.instance.Play(soundName, volume);
    }
}
