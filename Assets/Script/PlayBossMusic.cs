using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBossMusic : MonoBehaviour
{

    [SerializeField] float fadeTime = 4.0f;

    public AudioClip bossMusic;

    bool bossMusicPlaying;
    AudioClip levelMusic;

    // When the zone is entered by player, stop the song the camera is playing & fade to boss theme music.
    // When player leaves zone, fade back to level music.

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!bossMusicPlaying)
            {
                // Hang onto current clip so we can fade back to it if zone is left.
                levelMusic = Camera.main.GetComponent<AudioSource>().clip;

                // Fade to boss music
                StartCoroutine(FadeBetweenClips(Camera.main.GetComponent<AudioSource>(), fadeTime, bossMusic));
                bossMusicPlaying = true;
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (bossMusicPlaying)
            {
                // Fade back to level music
                StartCoroutine(FadeBetweenClips(Camera.main.GetComponent<AudioSource>(), fadeTime, levelMusic));
                bossMusicPlaying = false;
            }
        }
    }

    public static IEnumerator FadeBetweenClips(AudioSource audioSource, float fadeTime, AudioClip toClip)
    {
        // Fade out currently running clip
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
        audioSource.Stop();

        // Switch the clip 
        audioSource.clip = toClip;

        // Start playing the new clip (currently at 0 volume)
        audioSource.Play();

        // Fade new clip up to volume of original clip
        while(audioSource.volume < startVolume)
        {
            audioSource.volume += startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
    }
}


