using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    [Header("Tutorial Configuration")]
    [SerializeField] private List<TutorialMission> missions = new List<TutorialMission>();
    [SerializeField] private TMPro.TextMeshProUGUI missionDescriptionUI;

    [Header("Training Hub Configuration")]
    [SerializeField] private bool isTrainingMode = false;
    [SerializeField] private GameObject normalExitDoor;
    [SerializeField] private GameObject trainingHubExitDoor;

    [Header("Dependencies")]
    private DialogueManager _dialogueManager;
    private CameraTutorialManager _cameraTutorialManager;

    private TutorialMission _currentMission;
    private int _currentMissionIndex = 0;
    private TutorialState _currentState = TutorialState.NotStarted;
    private bool _dialogueStarted = false;
    private bool _tutorialCompleted = false;

    public static event Action<TutorialMission> OnMissionStarted;
    public static event Action<TutorialMission> OnMissionCompleted;
    public static event Action OnTutorialCompleted;
    public static event Action OnTutorialRestart;

    public bool IsTrainingMode => isTrainingMode;
    public bool IsTutorialCompleted => _tutorialCompleted;

    private void Awake()
    {
        ServiceLocator.Instance.RegisterService(this);
        LoadTrainingModeFromSave();
    }

    private void Start()
    {
        InitializeDependencies();
        SetupDoors();
        if (!isTrainingMode)
        {
            StartTutorial();
        }
        else
        {
            _tutorialCompleted = true;

            if (missionDescriptionUI != null)
            {
                missionDescriptionUI.text = "Training Mode - Explore freely or press E near the dummy to restart tutorial";
            }
        }
    }

    private void SetupDoors()
    {
        Debug.Log($"[SetupDoors] Training Mode: {isTrainingMode}");
        Debug.Log($"[SetupDoors] Normal Door: {normalExitDoor != null}, Training Door: {trainingHubExitDoor != null}");

        if (isTrainingMode)
        {
            if (normalExitDoor != null)
            {
                normalExitDoor.SetActive(false);
            }
            if (trainingHubExitDoor != null)
            {
                trainingHubExitDoor.SetActive(true);
            }
        }
        else
        {
            if (normalExitDoor != null)
            {
                normalExitDoor.SetActive(true);
            }
            if (trainingHubExitDoor != null)
            {
                trainingHubExitDoor.SetActive(false);
            }
        }
    }

    private void LoadTrainingModeFromSave()
    {
        SaveData saveData = SaveManager.Instance.GetCurrentSaveData();

        if (saveData != null)
        {
            isTrainingMode = saveData.isTrainingMode;
            Debug.Log($"Loaded training mode from save: {isTrainingMode}");
        }
    }

    private void InitializeDependencies()
    {
        _dialogueManager = ServiceLocator.Instance.GetService<DialogueManager>();
        _cameraTutorialManager = ServiceLocator.Instance.GetService<CameraTutorialManager>();

        if (_dialogueManager == null)
            Debug.LogError("DialogueManager not found!");

        if (_cameraTutorialManager == null)
            Debug.LogError("CameraTutorialManager not found!");
    }

    private void Update()
    {
        if (_currentMission != null && _currentState == TutorialState.WaitingForCompletion)
        {
            _currentMission.UpdateMission();

            if (_currentMission.IsCompleted())
            {
                CompleteMission();
            }
        }
    }

    public void SetTrainingMode(bool trainingMode)
    {
        isTrainingMode = trainingMode;

        if (trainingMode)
        {
            _currentState = TutorialState.NotStarted;
            _currentMission = null;

            if (missionDescriptionUI != null)
            {
                missionDescriptionUI.text = "Training Mode - Explore freely or press E near the dummy to restart tutorial";
            }
        }
    }

    public void RestartTutorial()
    {
        if (!_tutorialCompleted && !isTrainingMode)
            return;

        Debug.Log("Restarting tutorial from beginning...");

        _currentMissionIndex = 0;
        _currentMission = null;
        _currentState = TutorialState.NotStarted;
        _dialogueStarted = false;
        _tutorialCompleted = false;
        isTrainingMode = false;

        HideUIHint();
        OnTutorialRestart?.Invoke();
        StartTutorial();
    }

    private void StartTutorial()
    {
        Debug.Log("StartTutorial called");

        if (missions.Count > 0)
        {
            StartMission(0);
        }
        else
        {
            Debug.LogError("No missions configured for tutorial!");
        }
    }

    private void StartMission(int index)
    {
        if (index >= missions.Count)
        {
            CompleteTutorial();
            return;
        }

        _currentMissionIndex = index;
        _currentMission = missions[_currentMissionIndex];
        _currentState = TutorialState.NotStarted;
        _dialogueStarted = false;

        Debug.Log($"Starting Mission {index}: {_currentMission.missionName}");

        UpdateMissionDescription();

        OnMissionStarted?.Invoke(_currentMission);

        StartCoroutine(ProcessMissionFlow());
    }

    private IEnumerator ProcessMissionFlow()
    {
        _currentMission.Initialize(this);

        if (_currentMission.shouldMoveCamera && !_currentMission.cameraEvent.executeAfterDialogue)
        {
            _currentState = TutorialState.MovingCamera;
            yield return StartCoroutine(HandleCameraEvent(_currentMission.cameraEvent));
        }

        if (_currentMission.dialogueToPlay != EnumDialogues.None)
        {
            _currentState = TutorialState.ShowingDialogue;
            yield return StartCoroutine(HandleDialogue());
        }

        if (_currentMission.shouldMoveCamera && _currentMission.cameraEvent.executeAfterDialogue)
        {
            _currentState = TutorialState.MovingCamera;
            yield return StartCoroutine(HandleCameraEvent(_currentMission.cameraEvent));
        }

        if (_currentMission.showUIHint)
        {
            ShowUIHint(_currentMission.uiHintPrefab);
        }

        _currentState = TutorialState.WaitingForCompletion;

        if (_currentMission.autoCompleteDelay > 0)
        {
            yield return new WaitForSeconds(_currentMission.autoCompleteDelay);
            _currentMission.ForceComplete();
        }
    }

    private IEnumerator HandleDialogue()
    {
        if (_dialogueStarted) yield break;

        bool dialogueComplete = false;
        _dialogueStarted = true;

        _dialogueManager.OnConversationEnd = () => dialogueComplete = true;
        _dialogueManager.StartConversation(_currentMission.dialogueToPlay);

        yield return new WaitUntil(() => dialogueComplete);
    }

    private IEnumerator HandleCameraEvent(CameraEventConfig cameraConfig)
    {
        bool cameraComplete = false;
        _cameraTutorialManager.ExecuteCameraEvent(cameraConfig, () => cameraComplete = true);
        yield return new WaitUntil(() => cameraComplete);

        if (cameraConfig.shouldZoom)
        {
            yield return new WaitForSeconds(1f);
            bool resetComplete = false;
            _cameraTutorialManager.ResetCamera(() => resetComplete = true);
            yield return new WaitUntil(() => resetComplete);
        }
    }

    private void UpdateMissionDescription()
    {
        if (missionDescriptionUI != null && _currentMission != null)
        {
            missionDescriptionUI.text = _currentMission.missionDescription;
        }
    }

    private void CompleteMission()
    {
        if (_currentState == TutorialState.Completed)
            return;

        _currentState = TutorialState.Completed;

        Debug.Log($"Mission Completed: {_currentMission.missionName}");
        HideUIHint();
        OnMissionCompleted?.Invoke(_currentMission);
        _currentMission.Cleanup();
        StartMission(_currentMissionIndex + 1);
    }

    private void CompleteTutorial()
    {
        Debug.Log("Tutorial Completed!");
        _tutorialCompleted = true;

        if (missionDescriptionUI != null)
        {
            missionDescriptionUI.text = "Train or face your opponent";
        }

        OnTutorialCompleted?.Invoke();

        if (_cameraTutorialManager != null)
        {
            StartCoroutine(CompleteTutorialWithCameraZoom());
        }
        else if (_dialogueManager != null)
        {
            _dialogueManager.StartConversation(EnumDialogues.TutorialComplete);
        }
    }

    private IEnumerator CompleteTutorialWithCameraZoom()
    {
        if (_dialogueManager != null)
        {
            _dialogueManager.StartConversation(EnumDialogues.TutorialComplete);

            bool dialogueComplete = false;
            _dialogueManager.OnConversationEnd = () => dialogueComplete = true;
            yield return new WaitUntil(() => dialogueComplete);
        }

        CameraEventConfig exitCameraEvent = new CameraEventConfig
        {
            eventId = "Exit",
            targetTag = "Exit",
            targetName = "ExitDoor",
            moveDuration = 0.7f,
            shouldZoom = true,
            zoomAmount = 6f,
            zoomDuration = 0.3f
        };

        bool cameraComplete = false;
        _cameraTutorialManager.ExecuteCameraEvent(exitCameraEvent, () => cameraComplete = true);
        yield return new WaitUntil(() => cameraComplete);

        yield return new WaitForSeconds(1.5f);

        bool resetComplete = false;
        _cameraTutorialManager.ResetCamera(() => resetComplete = true);
        yield return new WaitUntil(() => resetComplete);
    }

    #region UI Hint Management
    private GameObject _currentUIHint;

    private void ShowUIHint(GameObject hintPrefab)
    {
        if (hintPrefab != null)
        {
            _currentUIHint = Instantiate(hintPrefab);
        }
    }

    private void HideUIHint()
    {
        if (_currentUIHint != null)
        {
            Destroy(_currentUIHint);
            _currentUIHint = null;
        }
    }
    #endregion

    #region Public Getters for UI
    public TutorialMission GetCurrentMission()
    {
        return _currentMission;
    }
    public List<TutorialMission> GetAllMissions()
    {
        return missions;
    }
    #endregion

    #region Context Menu Utilities
    [ContextMenu("Force Complete Current Mission")]
    public void ForceCompleteCurrentMission()
    {
        if (_currentMission != null && _currentState == TutorialState.WaitingForCompletion)
        {
            _currentMission.ForceComplete();
        }
    }

    [ContextMenu("Complete Tutorial")]
    public void CompleteTutorialFromMenu()
    {
        Debug.Log("Force completing entire tutorial from context menu...");
        if (_currentMission != null)
        {
            _currentMission.Cleanup();
        }

        HideUIHint();

        _currentMissionIndex = missions.Count;
        _currentMission = null;
        _currentState = TutorialState.Completed;

        CompleteTutorial();
    }

    [ContextMenu("Toggle Training Mode")]
    public void ToggleTrainingMode()
    {
        SetTrainingMode(!isTrainingMode);
    }

    [ContextMenu("Restart Tutorial")]
    public void RestartTutorialFromMenu()
    {
        RestartTutorial();
    }
    #endregion
}