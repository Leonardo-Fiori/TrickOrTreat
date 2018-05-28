using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager instance = null;

    private void OnEnable()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        source = gameObject.AddComponent<AudioSource>();
    }

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
    }

    public Sound[] sounds;
    private AudioSource source;

    private AudioClip Find(string soundName)
    {
        foreach(Sound sound in sounds)
        {
            if (sound.name == soundName)
                return sound.clip;
        }

        throw new System.Exception("Suono non trovato! " + "(" + name + ")");
    }

    public void Play(string name, float volume = 1f)
    {
        print("Playing " + name + " at volume " + volume + " ...");
        AudioClip clip = Find(name);
        source.PlayOneShot(clip, volume);
    }
}
