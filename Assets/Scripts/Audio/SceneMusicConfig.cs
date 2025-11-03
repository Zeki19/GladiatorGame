using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneMusicConfig", menuName = "Audio/Scene Music Config")]
public class SO_SceneMusicConfig : ScriptableObject
{
    [Serializable]
    public class SceneMusicMapping
    {
        [Tooltip("Exact scene name from Build Settings")]
        public string sceneName;

        [Tooltip("Sound name from the SO_Sounds playlist")]
        public string musicName;
    }

    [Header("Scene Music Configuration")]
    [Tooltip("Maps each scene to its corresponding music")]
    public SceneMusicMapping[] sceneMappings;

    [Header("Default Music (Optional)")]
    [Tooltip("Music to play if the scene is not in the mappings")]
    public string defaultMusicName;
    public string GetMusicForScene(string sceneName)
    {
        foreach (var mapping in sceneMappings)
        {
            if (mapping.sceneName == sceneName)
            {
                return mapping.musicName;
            }
        }

        return defaultMusicName;
    }
}