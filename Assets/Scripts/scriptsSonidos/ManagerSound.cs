using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerSound : MonoBehaviour
{
    [Header("Volume Settings")]
    //[Range(0f, 1f)] public float musicVolume = 0.5f;
    [Range(0f, 1f)] public float sfxVolume = 0.5f;

    [Header("Audio Sources")]
    //public AudioSource musicSource;
    public AudioSource sfxSourceSingle,sfxSourceLoop;
    public static ManagerSound instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
            Destroy(gameObject);
    }

    void Start()
    {
        //  musicSource.volume = musicVolume;
        sfxSourceSingle.volume = sfxVolume;
        sfxSourceLoop.volume = sfxVolume;
    }

    // Función para reproducir música
    public void PlayMusic(AudioClip clip)
    {
        //musicSource.clip = clip;
        //musicSource.Play();
    }

    // Función para reproducir efectos de sonido
    public void PlaySFX(AudioClip clip,bool loop)
    {
        if(!loop)
            sfxSourceSingle.PlayOneShot(clip);
        else
        {
            sfxSourceLoop.clip = clip;
            sfxSourceLoop.Play();
        }
    }

    public void StopAudioLoop() => sfxSourceLoop.Stop();

    // Función para cambiar el volumen de la música
    public void SetMusicVolume(float volume)
    {
        //musicVolume = volume;
        //musicSource.volume = musicVolume;
    }

    // Función para cambiar el volumen de los efectos de sonido
    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        sfxSourceSingle.volume = sfxVolume;
        sfxSourceLoop.volume = sfxVolume;
    }
}
