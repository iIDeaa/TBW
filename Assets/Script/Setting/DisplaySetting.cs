using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplaySetting : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;

    Resolution[] resolutions;

    void Awake()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new();

        int currentIndex = 0;

        for(int i=0;i<resolutions.Length;i++)
        {
            string option =
                resolutions[i].width +
                " x " +
                resolutions[i].height;

            options.Add(option);

            if(resolutions[i].width==Screen.currentResolution.width &&
               resolutions[i].height==Screen.currentResolution.height)
            {
                currentIndex=i;
            }
        }

        resolutionDropdown.AddOptions(options);

        int savedIndex =
            PlayerPrefs.GetInt("Resolution",currentIndex);

        resolutionDropdown.value=savedIndex;
        resolutionDropdown.RefreshShownValue();

        bool fullscreen =
            PlayerPrefs.GetInt("Fullscreen",1)==1;

        fullscreenToggle.isOn=fullscreen;

        ApplyResolution(savedIndex);
        ApplyFullscreen(fullscreen);
    }

    public void ApplyResolution(int index)
    {
        Resolution res=resolutions[index];

        Screen.SetResolution(
            res.width,
            res.height,
            Screen.fullScreen);

        PlayerPrefs.SetInt("Resolution",index);
    }

    public void ApplyFullscreen(bool value)
    {
        Screen.fullScreen=value;

        PlayerPrefs.SetInt(
            "Fullscreen",
            value?1:0);
    }
}
