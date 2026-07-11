using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSetting : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;

    public void SetMasterVolume(float value)
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);

        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    public void SetMusicVolume(float value)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);

        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void SetSFXVolume(float value)
    {
        mixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);

        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    public void Load()
    {
        float master = PlayerPrefs.GetFloat("MasterVolume",1);
        float music = PlayerPrefs.GetFloat("MusicVolume",1);
        float sfx = PlayerPrefs.GetFloat("SFXVolume",1);

        mixer.SetFloat("MasterVolume",Mathf.Log10(master)*20);
        mixer.SetFloat("MusicVolume",Mathf.Log10(music)*20);
        mixer.SetFloat("SFXVolume",Mathf.Log10(sfx)*20);
    }
}
