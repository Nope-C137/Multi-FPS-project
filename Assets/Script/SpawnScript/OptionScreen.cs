using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionScreen : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropDown;

    public Toggle fullScreenTog, vsyncTog;
    private Resolution[] resolutions;
    private List<Resolution> filterResolutions;

    private float currentRefreshRate;
    private int currentResolutionIndex = 0;
    private void Start()
    {
        fullScreenTog.isOn = Screen.fullScreen;

        if(QualitySettings.vSyncCount == 0)
        {
            vsyncTog.isOn = false;

        }
        else
        {
            vsyncTog.isOn = true;
        }

        resolutions = Screen.resolutions;
        filterResolutions = new List<Resolution>();

        resolutionDropDown.ClearOptions();
        currentRefreshRate = Screen.currentResolution.refreshRate;

        for(int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].refreshRate == currentRefreshRate)
            {
                filterResolutions.Add(resolutions[i]);
            }
        }
    }

    public void ApplyGraphic()
    {
        Screen.fullScreen = fullScreenTog.isOn;

        if (vsyncTog.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }
    }


}
