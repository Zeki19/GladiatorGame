using System;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int currentBossDefeated; 
    public int nextSceneToLoad;
    public bool isTrainingMode; 

    public string lastSaveTime;

    public SaveData()
    {
        currentBossDefeated = 0;
        nextSceneToLoad = 1; 
        isTrainingMode = false;
        lastSaveTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}

[System.Serializable]
public class SettingsData
{
    public float volume;
    public bool isFullscreen;
    public int resolutionIndex;

    public SettingsData()
    {
        volume = 0f; 
        isFullscreen = true;
        resolutionIndex = 0;
    }
}