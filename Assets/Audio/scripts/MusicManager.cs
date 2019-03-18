using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public SongsCollection songsCollection;

    private Song currentlyPlayingSong;

    private void Awake()
    {
        foreach (Song s in songsCollection.songs)
        {
            s.sources.Clear();

            foreach (AudioClip clip in s.tracks)
            {
                AudioSource source = gameObject.AddComponent<AudioSource>();
                source.clip = clip;
                source.volume = 1;
                source.pitch = 1;
                source.loop = true;
                s.sources.Add(source);
            }
        }
    }

    private void Start()
    {
        //PlayRandomSong();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.RightShift) && Input.GetKey(KeyCode.S)) 
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) { PlaySong(1); }
            if (Input.GetKeyDown(KeyCode.Alpha2)) { PlaySong(2); }
            if (Input.GetKeyDown(KeyCode.Alpha3)) { PlaySong(3); }
            if (Input.GetKeyDown(KeyCode.Alpha4)) { PlaySong(4); }
        }
    }

    void PlayRandomSong()
    {
        PlaySong(Random.Range(1, songsCollection.songs.Length+1));
    }

    void StopSong(Song songToStop)
    {
        foreach (AudioSource source in songToStop.sources)
        {
            source.Stop();
        }
    }

    void PlaySong(int index)
    {
        Song s = songsCollection.songs[index - 1];
        if (s == null)
        {
            Debug.Log(index + " is an invalid song index");
            return;
        }

        if (currentlyPlayingSong != null) { StopSong(currentlyPlayingSong); }

        Debug.Log("playing song: " + s.songName);

        foreach (AudioSource source in s.sources)
        {
            source.Play();
        }

        currentlyPlayingSong = s;
    }
}
