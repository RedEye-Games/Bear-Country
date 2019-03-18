using UnityEngine;
using UnityEngine.Audio;
using System;

/* This AudioManager handles all in-game SFX--except for the ambience and music--which are handled with an audio mixer. */

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField]
    private SoundsCollection sounds;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        DontDestroyOnLoad(gameObject);

        TileController.TileEvent += OnTileEvent;
        TileModifiers.TileEvent += OnTileEvent;
    }

    private void SetupSound(Sound s)
    {
        //Debug.Log("setting up sound " + s.soundName);
        s.source = gameObject.AddComponent<AudioSource>();
        s.source.clip = s.clip;
        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        s.source.loop = s.loop;
    }

    public void OnTileEvent(TileEventName tileEventName, GameObject tile)
    {
        switch (tileEventName)
        {
            case TileEventName.PickedUp:
            {
                if (tile.GetComponent<TileController>().isSpecial) Play(sounds.specialTilePickedUpSound);
                else Play(sounds.tilePickedUpSound);
                break;
            }
            case TileEventName.SuccessfullyPlaced: Play(sounds.tileSuccessfullyPlacedSound); break;
            case TileEventName.UnsuccessfullyPlaced: Play(sounds.tileUnsuccessfullyPlacedSound); break;
            case TileEventName.Flipped: Play(sounds.tileFlippedSound); break;
            case TileEventName.Rotated: Play(sounds.tileRotatedSound); break;
        }
    }

    public void Play(Sound s)
    {
        if (s.source == null) { SetupSound(s); }

        /* set the volume and pitch each time the sound is played, so that changes made to the 
         * scriptable objects while the game is running will be reflected.
         * These two lines can remove this for production build. */
        s.source.volume = s.volume;
        s.source.pitch = s.pitch;

        //Debug.Log("playing sound " + s.soundName);
        s.source.Play(); 
    }

    public void Play(Sound[] sounds)
    {
        foreach (Sound sound in sounds)
        {
            Play(sound);
        }
    }
}
