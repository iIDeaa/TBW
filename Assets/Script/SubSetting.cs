using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class SubSetting : MonoBehaviour
{
    [Header("Resolution Settings")]
    public TMP_Dropdown Resulution_dropdown;
    public Toggle Fullscreenop;
    private List<Resolution> uniqueResList = new List<Resolution>();

    [Header("Audio Settings")]
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider; 
    [SerializeField] private Slider sfxSlider;  
    
    void Start()
    {
        Resolution[] allResolutions = Screen.resolutions;
        System.Array.Reverse(allResolutions);
        Resulution_dropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentres = 0;
        List<string> createdOptions = new List<string>();

        for (int i = 0; i < allResolutions.Length; i++)
        {
            string optionText = allResolutions[i].width + " x " + allResolutions[i].height;

            if (!createdOptions.Contains(optionText))
            {
                createdOptions.Add(optionText); 
                uniqueResList.Add(allResolutions[i]); 
                options.Add(optionText); 

                if (allResolutions[i].width == Screen.width && allResolutions[i].height == Screen.height)
                {
                    currentres = uniqueResList.Count - 1;
                }
            }
        }
        
        Resulution_dropdown.AddOptions(options);
        Resulution_dropdown.value = currentres;
        Resulution_dropdown.RefreshShownValue();
        Fullscreenop.isOn = Screen.fullScreen;

        if (masterSlider != null)
        {
            float savedMaster = PlayerPrefs.GetFloat("MasterVolume", 1f);
            masterSlider.value = savedMaster;
            SetMasterVolume(savedMaster);
            masterSlider.onValueChanged.AddListener(SetMasterVolume);
        }

        if (musicSlider != null)
        {
            float savedMusic = PlayerPrefs.GetFloat("MusicVolume", 1f);
            musicSlider.value = savedMusic;
            SetMusicVolume(savedMusic);
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (sfxSlider != null)
        {
            float savedSFX = PlayerPrefs.GetFloat("SFXVolume", 1f);
            sfxSlider.value = savedSFX;
            SetSFXVolume(savedSFX);
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }
    }

    public void ChangeResolution(int index)
    {
        if (index >= 0 && index < uniqueResList.Count)
        {
            Resolution selectedRes = uniqueResList[index];
            Screen.SetResolution(selectedRes.width, selectedRes.height, Screen.fullScreen);
        }
    }
    
    public void ChangeFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetMasterVolume(float sliderValue)
    {
        if (myMixer != null)
        {
            float volumeInDb = Mathf.Log10(Mathf.Max(sliderValue, 0.0001f)) * 20;
            myMixer.SetFloat("MasterVolume", volumeInDb);
            PlayerPrefs.SetFloat("MasterVolume", sliderValue);
        }
    }

    public void SetMusicVolume(float sliderValue)
    {
        if (myMixer != null)
        {
            float volumeInDb = Mathf.Log10(Mathf.Max(sliderValue, 0.0001f)) * 20;
            myMixer.SetFloat("MusicVolume", volumeInDb); 
            PlayerPrefs.SetFloat("MusicVolume", sliderValue);
        }
    }

    public void SetSFXVolume(float sliderValue)
    {
        if (myMixer != null)
        {
            float volumeInDb = Mathf.Log10(Mathf.Max(sliderValue, 0.0001f)) * 20;
            myMixer.SetFloat("SFXVolume", volumeInDb); 
            PlayerPrefs.SetFloat("SFXVolume", sliderValue);
        }
    }
}