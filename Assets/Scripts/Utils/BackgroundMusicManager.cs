using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> playlist;

    private int currentTrackIndex = 0;

    private void Start()
    {
        if (playlist.Count > 0)
        {
            PlayCurrentSong();
        }
    }

    private void Update()
    {
        if (!audioSource.isPlaying && playlist.Count > 0)
        {
            PlayNextSong();
        }
    }

    private void PlayCurrentSong()
    {
        audioSource.clip = playlist[currentTrackIndex];
        audioSource.Play();
    }

    private void PlayNextSong()
    {
        currentTrackIndex++;

        if (currentTrackIndex >= playlist.Count)
        {
            currentTrackIndex = 0; // Restart from the first track
        }

        PlayCurrentSong();
    }
}