using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class VolumeSettings : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private List<Volumes> volumes = new List<Volumes>();
    private readonly List<string> _options = new List<string>();
    [Header("Wiring")]
    [SerializeField] private AudioMixer masterMixer;
    [SerializeField] private TMP_Dropdown masterDropdown;
    [SerializeField] private TMP_Dropdown soundDropdown;
    [SerializeField] private TMP_Dropdown musicDropdown;
    
    private const string Master = "volume";
    private const string Music = "music";
    private const string Sound = "Sound";
    
    private void Awake()
    {
        foreach (var volume in volumes)
        {
            _options.Add(volume.label);
        }
        
        SetupDropdown(masterDropdown);
        SetupDropdown(musicDropdown);
        SetupDropdown(soundDropdown);
    }

    private void OnEnable()
    {
        masterMixer.GetFloat(Master, out float masterVolume);
        var masterIndex = VolumesListIndex(masterVolume);
        masterDropdown.value = masterIndex != -1 ? masterIndex : 0;
        masterDropdown.RefreshShownValue();
        
        masterMixer.GetFloat(Music, out float musicVolume);
        var musicIndex = VolumesListIndex(musicVolume);
        musicDropdown.value = musicIndex != -1 ? musicIndex : 0;
        musicDropdown.RefreshShownValue();
        
        masterMixer.GetFloat(Sound, out float soundVolume);
        var soundIndex =  VolumesListIndex(soundVolume);
        soundDropdown.value = soundIndex != -1 ? soundIndex : 0;
        soundDropdown.RefreshShownValue();
    }

    private void SetupDropdown(TMP_Dropdown dropdown)
    {
        if (!dropdown) return;
        
        dropdown.ClearOptions();
        dropdown.AddOptions(_options);
        dropdown.RefreshShownValue();
    }
    public void SetMusicVolume()
    {
        var volume = VolumeValue(musicDropdown.value);
        masterMixer.SetFloat(Music, volume);
    }
    public void SetSoundsVolume()
    {
        var volume = VolumeValue(soundDropdown.value);
        masterMixer.SetFloat(Sound, volume);
    }
    public void SetMasterVolume()
    {
        var volume = VolumeValue(masterDropdown.value);
        masterMixer.SetFloat(Master, volume);
    }
    private float VolumeValue(int value)
    {
        float v = volumes[value].volume;
        return v;
    }
    private int VolumesListIndex(float value)
    {
        for (int i = 0; i < volumes.Count; i++)
        {
            if (Mathf.Approximately(volumes[i].volume, value))
            {
                return i;
            }
        }

        return -1;
    }

    [Serializable]
    struct Volumes
    {
        public string label;
        public float volume;
    }
}

