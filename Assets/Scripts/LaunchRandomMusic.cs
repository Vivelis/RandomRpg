using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class LaunchRandomMusic : MonoBehaviour
{
    [SerializeField] private AudioClip[] musicClips = null;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;
        PlayRandomMusic();
        StartCoroutine(PlayNextMusicWhenFinished());
    }

    private IEnumerator PlayNextMusicWhenFinished()
    {
        while (true)
        {
            yield return new WaitUntil(() => !audioSource.isPlaying);
            PlayRandomMusic();
        }
    }

    private void PlayRandomMusic()
    {
        audioSource.clip = musicClips[UnityEngine.Random.Range(0, musicClips.Length)];
        audioSource.Play();
    }
}
