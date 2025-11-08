using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SceneMusicConfig", menuName = "Audio/Scene Music Config")]
public class SO_SceneMusicConfig : ScriptableObject
{
    [Serializable]
    public class SceneMusicMapping
    {
        [Tooltip("Exact scene name from Build Settings")]
        public string sceneName;

        [Tooltip("Sound asset to play for this scene")]
        public Sound music;
    }

    [Header("Scene Music Configuration")]
    [Tooltip("Maps each scene to its corresponding music")]
    public SceneMusicMapping[] sceneMappings;

    [Header("Default Music (Optional)")]
    [Tooltip("Music to play if the scene is not in the mappings")]
    public Sound defaultMusic;

    public Sound GetMusicForScene(string sceneName)
    {
        foreach (var mapping in sceneMappings)
        {
            if (mapping.sceneName == sceneName)
            {
                return mapping.music;
            }
        }
        return defaultMusic;
    }
}