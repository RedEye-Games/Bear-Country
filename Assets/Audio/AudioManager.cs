using UnityEngine;
using UnityEngine.Audio;
using System;


/* This AudioManager handles all in-game SFX--except for the ambience and music--which are handled with an audio mixer. */

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Sound[] sounds;

    private void Awake()
    {
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }

        DontDestroyOnLoad(gameObject);
        SetupAllSounds();
    }

    private void SetupAllSounds()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) { Debug.LogWarning("cannot find sound named " + name); }
        else { s.source.Play(); }
    }
}
