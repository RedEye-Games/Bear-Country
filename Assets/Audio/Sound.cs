using UnityEngine;
using UnityEngine.Audio;
using System;

[Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0.0f, 1.0f)]
    public float volume = 1.0f;

    [Range(0.1f, 2.0f)]
    public float pitch = 1.0f;

    [HideInInspector]
    public AudioSource source;

    public bool loop = false;
}
