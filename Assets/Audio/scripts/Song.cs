using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "audio/song")]
public class Song : ScriptableObject 
{
    public string songName;
    public AudioClip[] tracks;

    [HideInInspector] 
    public List<AudioSource> sources;
}
