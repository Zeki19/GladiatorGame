using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Scriptables")]
    [SerializeField] private List<AudioSettingsScriptable> audioClips;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSourcePrefab;
    private AudioSource musicSource;

    private Dictionary<string, AudioSettingsScriptable> clipLookup = new Dictionary<string, AudioSettingsScriptable>();
    private List<AudioSource> activeSfxSources = new List<AudioSource>();

    [Header("Volume Settings")]
    [Range(0f, 100f)][SerializeField] private float musicVolume = 1f;
    [Range(0f, 100f)][SerializeField] private float sfxVolume = 1f;

    private void Awake()
    {
        ServiceLocator.Instance.RegisterService<AudioManager>(this);

        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
        }

        foreach (var clipSetting in audioClips)
        {
            if (clipSetting != null && !clipLookup.ContainsKey(clipSetting.soundName))
            {
                clipLookup.Add(clipSetting.soundName, clipSetting);
            }
        }
    }
    private void Update()
    {
        for (int i = activeSfxSources.Count - 1; i >= 0; i--)
        {
            if (!activeSfxSources[i].isPlaying)
            {
                Destroy(activeSfxSources[i].gameObject);
                activeSfxSources.RemoveAt(i);
            }
        }

        musicSource.volume = musicVolume;
        foreach (var sfx in activeSfxSources)
        {
            sfx.volume = sfxVolume;
        }
    }
    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        musicSource.clip = clip;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }
    public void PlayMusic(string soundName)
    {
        if (clipLookup.TryGetValue(soundName, out var soundData))
        {
            musicSource.clip = soundData.clip;
            musicSource.volume = soundData.volume * musicVolume;
            musicSource.Play();
        }
    }
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        var newSource = Instantiate(sfxSourcePrefab, transform);
        newSource.clip = clip;
        newSource.volume = sfxVolume;
        newSource.Play();

        activeSfxSources.Add(newSource);
    }
    public void PlaySFX(string soundName)
    {
        if (clipLookup.TryGetValue(soundName, out var soundData))
        {
            var newSource = Instantiate(sfxSourcePrefab, transform);
            newSource.clip = soundData.clip;
            newSource.volume = soundData.volume * sfxVolume;
            newSource.Play();

            activeSfxSources.Add(newSource);
        }
    }
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
    }
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
    }
}