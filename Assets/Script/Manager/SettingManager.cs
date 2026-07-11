using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    [Header("Audio")]

    [SerializeField] AudioSetting audioSettings;

    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    void Start()
    {
        float master =
            PlayerPrefs.GetFloat("MasterVolume",1);

        float music =
            PlayerPrefs.GetFloat("MusicVolume",1);

        float sfx =
            PlayerPrefs.GetFloat("SFXVolume",1);

        masterSlider.value=master;
        musicSlider.value=music;
        sfxSlider.value=sfx;

        audioSettings.Load();
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}
