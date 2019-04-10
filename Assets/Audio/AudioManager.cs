using UnityEngine;
using UnityEngine.Audio;
using System;

/* This AudioManager handles all in-game SFX--except for the ambience and music--which are handled with an audio mixer. */

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public Sound[] sounds;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        DontDestroyOnLoad(gameObject);
        SetupAllSounds();

        TileController.TileEvent += OnTileEvent;
        TileModifiers.TileEvent += OnTileEvent;
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

    public void OnTileEvent(TileEventName tileEventName, GameObject tile)
    {
        switch (tileEventName)
        {
            case TileEventName.PickedUp:
                {
                    if (tile.GetComponent<TileController>().isSpecial) Play("clickPop");
                    Play("Click1");
                    break;
                }
            case TileEventName.SuccessfullyPlaced: Play("Uh-huh"); Play("drop"); break;
            case TileEventName.UnsuccessfullyPlaced: Play("Click1Low"); break;
            case TileEventName.Flipped: Play("Click1"); break;
            case TileEventName.Rotated: Play("Click1"); break;
        }
    }

 
   public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) { Debug.LogWarning("cannot find sound named " + name); }
        else { s.source.Play(); }
    }
}
