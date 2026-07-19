using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSetting : MonoBehaviour
{
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer mixer;


    [Header("Volume Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;


    private const string MASTER = "MasterVolume";
    private const string MUSIC = "MusicVolume";
    private const string SFX = "SFXVolume";


    private void Start()
    {
        
        Load();
    }


    // ==========================
    // Master Volume
    // ==========================

    public void SetMasterVolume(float value)
    {
        value = FixVolume(value);

        mixer.SetFloat(
            MASTER,
            Mathf.Log10(value) * 20
        );

        PlayerPrefs.SetFloat(
            MASTER,
            value
        );
    }


    // ==========================
    // Music Volume
    // ==========================

    public void SetMusicVolume(float value)
    {
        value = FixVolume(value);

        mixer.SetFloat(
            MUSIC,
            Mathf.Log10(value) * 20
        );

        PlayerPrefs.SetFloat(
            MUSIC,
            value
        );
    }


    // ==========================
    // SFX Volume
    // ==========================

    public void SetSFXVolume(float value)
    {
        Debug.Log("SFX Slider = " + value);

        value = FixVolume(value);

        mixer.SetFloat(
            "SFXVolume",
            Mathf.Log10(value) * 20
        );

        PlayerPrefs.SetFloat(
            "SFXVolume",
            value
        );
    }


    // ==========================
    // Load Setting
    // ==========================

    public void Load()
    {
        float master =
            PlayerPrefs.GetFloat(MASTER, 1f);

        float music =
            PlayerPrefs.GetFloat(MUSIC, 1f);

        float sfx =
            PlayerPrefs.GetFloat(SFX, 1f);



        mixer.SetFloat(
            MASTER,
            Mathf.Log10(master) * 20
        );

        mixer.SetFloat(
            MUSIC,
            Mathf.Log10(music) * 20
        );

        mixer.SetFloat(
            SFX,
            Mathf.Log10(sfx) * 20
        );


        // Update Slider
        if(masterSlider != null)
            masterSlider.value = master;

        if(musicSlider != null)
            musicSlider.value = music;

        if(sfxSlider != null)
            sfxSlider.value = sfx;
    }


    // ==========================
    // Prevent Log10(0)
    // ==========================

    private float FixVolume(float value)
    {
        if(value <= 0)
            value = 0.0001f;

        return value;
    }
}