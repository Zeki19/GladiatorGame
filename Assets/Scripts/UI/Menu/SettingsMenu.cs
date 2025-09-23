using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private int toScene;
    private Resolution[] _resolutions;

    private void Start()
    {
        _resolutions = Screen.resolutions;
        
        resolutionDropdown.ClearOptions();
        
        List<string> options = new List<string>();
        
        int currentResolutionIndex = 0;

        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + " x " + _resolutions[i].height;
            options.Add(option);

            if (_resolutions[i].width == Screen.currentResolution.width && _resolutions[i].height == Screen.currentResolution.height)
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
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetFullscreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
    }

    public void CloseInGameMenu()
    {
        gameObject.SetActive(false);
    }

    public void ChangeScene()
    {
        ServiceLocator.Instance.GetService<SceneChanger>().ChangeScene(toScene);
    }
}
