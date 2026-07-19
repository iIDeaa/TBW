using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Sound
{
    public string name;

    public AudioClip[] clips;

    [Range(0f, 1f)]
    public float volume = 1f;

    [Range(0.5f, 2f)]
    public float pitch = 1f;
}


public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;


    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;


    [Header("Sound Library")]
    [SerializeField] private List<Sound> sounds = new();


    private Dictionary<string, Sound> soundDictionary;


    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }


        CreateDictionary();
    }


    private void CreateDictionary()
    {
        soundDictionary = new Dictionary<string, Sound>();

        foreach (Sound sound in sounds)
        {
            if (string.IsNullOrEmpty(sound.name))
                continue;


            if (!soundDictionary.ContainsKey(sound.name))
            {
                soundDictionary.Add(sound.name, sound);
            }
            else
            {
                Debug.LogWarning(
                    "Sound name ซ้ำ : " + sound.name
                );
            }
        }
    }



    // =========================
    // SFX
    // =========================

    public void PlaySFX(string soundName)
    {
        if (!soundDictionary.TryGetValue(soundName, out Sound sound))
        {
            Debug.LogWarning(
                "หาเสียงไม่เจอ : " + soundName
            );
            return;
        }


        if (sound.clips.Length == 0)
            return;


        AudioClip clip =
            sound.clips[
                UnityEngine.Random.Range(
                    0,
                    sound.clips.Length
                )
            ];


        sfxSource.pitch = sound.pitch;

        sfxSource.PlayOneShot(
            clip,
            sound.volume
        );


        sfxSource.pitch = 1f;
    }



    // =========================
    // MUSIC
    // =========================

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (clip == null)
            return;


        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }


    public void StopMusic()
    {
        musicSource.Stop();
    }


    public void PauseMusic()
    {
        musicSource.Pause();
    }


    public void ResumeMusic()
    {
        musicSource.UnPause();
    }
}