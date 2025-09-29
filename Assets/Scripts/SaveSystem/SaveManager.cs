using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static SaveManager _instance;
    public static SaveManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("SaveManager");
                _instance = go.AddComponent<SaveManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    private SaveData _currentSaveData;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            SaveSystem.Init();
            LoadGameData();
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ServiceLocator.Instance.RegisterService(this);
    }

    public void LoadGameData()
    {
        _currentSaveData = SaveSystem.LoadGame();

        if (_currentSaveData == null)
        {
            _currentSaveData = new SaveData();
            Debug.Log("No save data found, created new save data");
        }
    }

    public SaveData GetCurrentSaveData()
    {
        if (_currentSaveData == null)
        {
            LoadGameData();
        }
        return _currentSaveData;
    }

    public void SaveAfterTutorialComplete()
    {
        _currentSaveData.currentBossDefeated = 0;
        _currentSaveData.nextSceneToLoad = 2; 
        _currentSaveData.isTrainingMode = false;
        _currentSaveData.lastSaveTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        SaveSystem.SaveGame(_currentSaveData);
        Debug.Log("Game saved after tutorial completion. Next scene: Boss 1");
    }

    public void SaveAfterBossDefeat(int bossNumber)
    {
        _currentSaveData.currentBossDefeated = bossNumber;
        _currentSaveData.isTrainingMode = true; 

        if (bossNumber == 1)
        {
            _currentSaveData.nextSceneToLoad = 3;
        }
        else if (bossNumber == 2)
        {
            _currentSaveData.nextSceneToLoad = 4;
        }
        else if (bossNumber == 3)
        {
            _currentSaveData.nextSceneToLoad = 0; 
        }

        _currentSaveData.lastSaveTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        SaveSystem.SaveGame(_currentSaveData);
        Debug.Log($"Game saved after defeating Boss {bossNumber}. Training mode: ON, Next scene: {_currentSaveData.nextSceneToLoad}");
    }


    public void SaveBeforeLeavingTrainingHub(int nextBossScene)
    {
        _currentSaveData.isTrainingMode = false; 
        _currentSaveData.lastSaveTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        SaveSystem.SaveGame(_currentSaveData);
        Debug.Log($"Game saved before leaving training hub. Training mode OFF, Next scene: {nextBossScene}");
    }

    public void SaveSettings(float volume, bool fullscreen, int resolutionIndex)
    {
        SettingsData settings = new SettingsData
        {
            volume = volume,
            isFullscreen = fullscreen,
            resolutionIndex = resolutionIndex
        };

        SaveSystem.SaveSettings(settings);
        Debug.Log("Settings saved");
    }

    public SettingsData LoadSettings()
    {
        return SaveSystem.LoadSettings();
    }

    public void DeleteAllSaves()
    {
        SaveSystem.DeleteAllSaves();
        _currentSaveData = new SaveData();
        Debug.Log("All saves deleted and reset to default");
    }

    public bool HasSaveData()
    {
        return SaveSystem.SaveExists();
    }

    public void StartNewGame()
    {
        _currentSaveData = new SaveData();
        SaveSystem.SaveGame(_currentSaveData);
        Debug.Log("New game started");
    }
}