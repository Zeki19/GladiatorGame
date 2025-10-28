using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplaySettings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;

    private Resolution[] _resolutions;

    private void Start()
    {
        SetupResolutions();
    }

    private void SetupResolutions()
    {
        _resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        List<Resolution> uniqueResolutions = new List<Resolution>();

        int currentResolutionIndex = 0;

        foreach (var res in _resolutions)
        {
            bool exists = uniqueResolutions.Exists(r => r.width == res.width && r.height == res.height);
            if (!exists)
            {
                uniqueResolutions.Add(res);
            }
        }

        for (int i = 0; i < uniqueResolutions.Count; i++)
        {
            string option = uniqueResolutions[i].width + " x " + uniqueResolutions[i].height;
            options.Add(option);

            if (uniqueResolutions[i].width == Screen.currentResolution.width &&
                uniqueResolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullscreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
    }
}
