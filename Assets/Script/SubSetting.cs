using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SubSetting : MonoBehaviour
{
    public TMP_Dropdown Resulution_dropdown;
    public Toggle Fullscreenop;

    // เปลี่ยนมาใช้ List เพื่อเก็บเฉพาะขนาดที่ผ่านการกรองแล้ว
    private List<Resolution> uniqueResList = new List<Resolution>();
    
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
}