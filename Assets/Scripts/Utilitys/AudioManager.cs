using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clip Configurations")]
    [SerializeField] private List<AudioSettingsScriptable> audioClips;
    private Dictionary<string, AudioSettingsScriptable> clipLookup;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        clipLookup = audioClips.ToDictionary(c => c.soundName, c => c);
    }
    public void PlaySFX(string id)
    {
        if (clipLookup.TryGetValue(id, out var data))
        {
            sfxSource.PlayOneShot(data.clip, data.volume);
        }
        else
        {
            Debug.LogWarning($"SID sfx '{id}' Not Found");
        }
    }
    public void PlayMusic(string id)
    {
        if (clipLookup.TryGetValue(id, out var data))
        {
            sfxSource.clip = data.clip;
            sfxSource.volume = data.volume;
            sfxSource.loop = true;
            sfxSource.Play();
        }
        else
        {
            Debug.LogWarning($"ID music '{id}' Not found ");
        }
    }
    public void StopMusic()
    {
        sfxSource.Stop();
    }
}