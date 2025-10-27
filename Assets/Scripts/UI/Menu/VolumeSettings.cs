using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [Header("Sound settings")]
    [SerializeField] private AudioMixer masterMixer;
    private const string Master = "volume";
    private const string Music = "music";
    private const string Sfx = "Sound";
    [Header("Music wiring")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        masterMixer.SetFloat(Music, volume);
    }
    public void SetSfxVolume()
    {
        float volume = sfxSlider.value;
        masterMixer.SetFloat(Sfx, volume);
    }
    public void SetMasterVolume()
    {
        float volume = masterSlider.value;
        masterMixer.SetFloat(Master, volume);
    }
}

