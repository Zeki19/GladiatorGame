using UnityEngine;

[CreateAssetMenu(fileName = "AudioSettings", menuName = "Settings/Audio Settings")]
public class AudioSettingsScriptable : ScriptableObject
{
    public string soundName;
    public AudioClip clip;
    [Range(0f, 100f)] public float volume = 1f;
}