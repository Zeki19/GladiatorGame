using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static readonly string SAVE_FOLDER = Application.persistentDataPath + "/GameSaves/";
    private const string SAVE_FILE_NAME = "gamesave.json";
    private const string SETTINGS_FILE_NAME = "settings.json";
    private static bool _isInitialized = false;

    public static void Init()
    {
        if (_isInitialized)
            return;

        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }
        Debug.Log($"Save folder initialized at: {SAVE_FOLDER}");
        _isInitialized = true;
    }

    public static void SaveGame(SaveData data)
    {
        Init();
        string json = JsonUtility.ToJson(data, true);
        string fullPath = SAVE_FOLDER + SAVE_FILE_NAME;
        File.WriteAllText(fullPath, json);
        Debug.Log($"Game saved to: {fullPath}");
    }

    public static SaveData LoadGame()
    {
        Init();
        string fullPath = SAVE_FOLDER + SAVE_FILE_NAME;

        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("Game loaded successfully");
            return data;
        }

        Debug.Log("No save file found, returning new save data");
        return null;
    }

    public static void SaveSettings(SettingsData data)
    {
        Init();
        string json = JsonUtility.ToJson(data, true);
        string fullPath = SAVE_FOLDER + SETTINGS_FILE_NAME;
        File.WriteAllText(fullPath, json);
        Debug.Log($"Settings saved to: {fullPath}");
    }

    public static SettingsData LoadSettings()
    {
        Init();
        string fullPath = SAVE_FOLDER + SETTINGS_FILE_NAME;

        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            SettingsData data = JsonUtility.FromJson<SettingsData>(json);
            Debug.Log("Settings loaded successfully");
            return data;
        }

        Debug.Log("No settings file found, returning default settings");
        return new SettingsData();
    }

    public static bool SaveExists()
    {
        Init();
        return File.Exists(SAVE_FOLDER + SAVE_FILE_NAME);
    }

    public static void DeleteAllSaves()
    {
        Init();
        string gameSavePath = SAVE_FOLDER + SAVE_FILE_NAME;

        if (File.Exists(gameSavePath))
        {
            File.Delete(gameSavePath);
            Debug.Log("Game save deleted");
        }

        string settingsPath = SAVE_FOLDER + SETTINGS_FILE_NAME;

        if (File.Exists(settingsPath))
        {
            File.Delete(settingsPath);
            Debug.Log("Settings deleted");
        }

        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("All saves and preferences deleted");
    }

    public static string GetSaveFolderPath()
    {
        return SAVE_FOLDER;
    }
}