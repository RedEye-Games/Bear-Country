using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "audio/sound")]
public class Sound : ScriptableObject
{
    public string soundName;

    public AudioClip clip;

    [Range(0.0f, 1.0f)]
    public float volume = 1.0f;

    [Range(0.1f, 2.0f)]
    public float pitch = 1.0f;

    [HideInInspector]
    public AudioSource source;

    public bool loop = false;
}
