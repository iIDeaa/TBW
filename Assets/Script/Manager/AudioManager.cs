using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

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

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer mixer;

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
            
            // สมัครรับ Event เมื่อเปลี่ยนซีน เพื่อให้ตั้งค่าเสียงใหม่ทุกครั้งที่โหลดซีนเสร็จ
            SceneManager.sceneLoaded += OnSceneLoaded;
            
            // โหลดและนำการตั้งค่าทั้งหมดมาใช้ตอนเปิดเกมครั้งแรก
            ApplyAllSettings();
        }
        else
        {
            Destroy(gameObject);
            return;
        }


        CreateDictionary();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // เมื่อเปลี่ยนซีน ให้ Apply เสียงซ้ำอีกครั้ง เพื่อป้องกัน AudioMixer คืนค่าเริ่มต้น
        ApplyAudioSettings();
    }

    public void ApplyAllSettings()
    {
        ApplyAudioSettings();
        ApplyDisplaySettings();
    }

    public void ApplyAudioSettings()
    {
        if (mixer == null) return;

        float master = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float music = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 1f);

        mixer.SetFloat("MasterVolume", Mathf.Log10(Mathf.Max(master, 0.0001f)) * 20);
        mixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Max(music, 0.0001f)) * 20);
        mixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Max(sfx, 0.0001f)) * 20);
    }

    public void ApplyDisplaySettings()
    {
        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        Screen.fullScreen = isFullscreen;

        if (PlayerPrefs.HasKey("Resolution"))
        {
            int resIndex = PlayerPrefs.GetInt("Resolution");
            Resolution[] resolutions = Screen.resolutions;
            if (resIndex >= 0 && resIndex < resolutions.Length)
            {
                Resolution res = resolutions[resIndex];
                Screen.SetResolution(res.width, res.height, isFullscreen);
            }
        }
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