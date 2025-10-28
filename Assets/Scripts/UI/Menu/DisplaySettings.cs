using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplaySettings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;

    private Resolution[] _resolutions;
    private bool _currentFullscreen;
    private int _currentResolutionIndex;

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
        _currentResolutionIndex = resolutionIndex;
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        Debug.Log(resolution);
    }

    public void SetFullscreen(bool fullscreen)
    {
        _currentFullscreen = fullscreen;
        Screen.fullScreen = fullscreen;
        Debug.Log("FS: "+ _currentFullscreen);
    }
}
