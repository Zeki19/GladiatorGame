using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Toggle fullscreenToggle;

    private Resolution[] _resolutions;
    private float _currentVolume;
    private bool _currentFullscreen;
    private int _currentResolutionIndex;

    private void Start()
    {
        SetupResolutions();
        LoadSettings();
    }

    private void SetupResolutions()
    {
        _resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + " x " + _resolutions[i].height;
            options.Add(option);

            if (_resolutions[i].width == Screen.currentResolution.width &&
                _resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    private void LoadSettings()
    {
        SettingsData settings = SaveManager.Instance.LoadSettings();

        _currentVolume = settings.volume;
        audioMixer.SetFloat("volume", _currentVolume);
        if (volumeSlider != null)
            volumeSlider.value = _currentVolume;

        _currentFullscreen = settings.isFullscreen;
        Screen.fullScreen = _currentFullscreen;
        if (fullscreenToggle != null)
            fullscreenToggle.isOn = _currentFullscreen;

        _currentResolutionIndex = settings.resolutionIndex;
        if (_currentResolutionIndex >= 0 && _currentResolutionIndex < _resolutions.Length)
        {
            Resolution resolution = _resolutions[_currentResolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, _currentFullscreen);
            resolutionDropdown.value = _currentResolutionIndex;
        }
    }

    public void SetResolution(int resolutionIndex)
    {
        _currentResolutionIndex = resolutionIndex;
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        SaveCurrentSettings();
    }

    public void SetVolume(float volume)
    {
        _currentVolume = volume;
        audioMixer.SetFloat("volume", volume);
        SaveCurrentSettings();
    }

    public void SetFullscreen(bool fullscreen)
    {
        _currentFullscreen = fullscreen;
        Screen.fullScreen = fullscreen;
        SaveCurrentSettings();
    }

    private void SaveCurrentSettings()
    {
        SaveManager.Instance.SaveSettings(_currentVolume, _currentFullscreen, _currentResolutionIndex);
    }

    public void CloseInGameMenu()
    {
        gameObject.SetActive(false);
    }
}