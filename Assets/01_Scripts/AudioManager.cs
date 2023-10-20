using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Referencias")]
    public AudioSource musixAs;
    public AudioSource sfxAS;

    [Header("Propiedades")]

    [Range(0f, 1f)] public float musicVol = 0.5f;
    [Range(0f, 1f)] public float sfxVol = 1.0f;
    public static AudioManager instance;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


        musixAs.volume = musicVol;
        musixAs.playOnAwake = true;
        musixAs.loop = true;

        sfxAS.volume = sfxVol;
        sfxAS.playOnAwake = false;
        sfxAS.loop = false;
    }


    public void PlaySong(AudioClip sound)
    {
        sfxAS.PlayOneShot(sound);
    }

    public void SetMusic(AudioClip song)
    {
        musixAs.Stop();
        musixAs.clip = song;
        musixAs.Play();
    }

    public void SetMusicNotStop(AudioClip song)
    {
        musixAs.clip = song;
        musixAs.Play();
    }

    public void SongStop(AudioClip song)
    {
        musixAs.Stop();
    }
}
