using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    [Header("Tutorial Configuration")]
    [SerializeField] private List<TutorialMission> missions = new List<TutorialMission>();

    [Header("Training Hub Configuration")]
    [SerializeField] private bool isTrainingMode = false;
    [SerializeField] private GameObject normalExitDoor;
    [SerializeField] private GameObject trainingHubExitDoor;

    [Header("UI References")]
    [Tooltip("Button that allows skipping to the end of the tutorial")]
    [SerializeField] private GameObject skipTutorialButton;

    [Header("Dependencies")]
    [SerializeField] private DialogueManager dialogueManager;
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
        
        UpdateSkipButtonVisibility();
        
        if (!isTrainingMode)
        {
            StartTutorial();
        }
        else
        {
            _tutorialCompleted = true;
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
        _cameraTutorialManager = ServiceLocator.Instance.GetService<CameraTutorialManager>();

        if (dialogueManager == null) dialogueManager = ServiceLocator.Instance.GetService<DialogueManager>();

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
        }
        
        UpdateSkipButtonVisibility();
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
        UpdateSkipButtonVisibility(); 
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
        
        UpdateSkipButtonVisibility(); 
        OnMissionStarted?.Invoke(_currentMission);

        StartCoroutine(ProcessMissionFlow());
    }

    private IEnumerator ProcessMissionFlow()
    {
        _currentMission.Initialize(this);

        Debug.Log($"[TutorialManager] Pausing game for mission: {_currentMission.missionName}");

        yield return null;

        yield return new WaitForSeconds(1f);

        PauseManager.SetPausedCinematic(true);

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

        Debug.Log($"[TutorialManager] Unpausing game - player can now complete mission: {_currentMission.missionName}");
        PauseManager.SetPausedCinematic(false);

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

        dialogueManager.OnConversationEnd = () => dialogueComplete = true;
        dialogueManager.StartConversation(_currentMission.dialogueToPlay);

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

        UpdateSkipButtonVisibility();
        OnTutorialCompleted?.Invoke();

        if (_cameraTutorialManager != null)
        {
            StartCoroutine(CompleteTutorialWithCameraZoom());
        }
        else if (dialogueManager != null)
        {
            dialogueManager.StartConversation(EnumDialogues.TutorialComplete);
        }
    }

    private IEnumerator CompleteTutorialWithCameraZoom()
    {
        PauseManager.SetPausedCinematic(true);

        if (dialogueManager != null)
        {
            dialogueManager.StartConversation(EnumDialogues.TutorialComplete);

            bool dialogueComplete = false;
            dialogueManager.OnConversationEnd = () => dialogueComplete = true;
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
        PauseManager.SetPausedCinematic(false);
    }

    #region Skip Button Management

    private void UpdateSkipButtonVisibility()
    {
        if (skipTutorialButton == null)
            return;
        bool shouldShowButton = !isTrainingMode && 
                               !_tutorialCompleted && 
                               (_currentMission != null || _currentMissionIndex == 0);

        skipTutorialButton.SetActive(shouldShowButton);
        
        Debug.Log($"[TutorialManager] Skip button visibility: {shouldShowButton} " +
                  $"(TrainingMode: {isTrainingMode}, Completed: {_tutorialCompleted}, HasMission: {_currentMission != null})");
    }

    public void SetSkipButton(GameObject button)
    {
        skipTutorialButton = button;
        UpdateSkipButtonVisibility();
    }
    #endregion

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

    [ContextMenu("Toggle Skip Button")]
    public void ToggleSkipButton()
    {
        if (skipTutorialButton != null)
        {
            skipTutorialButton.SetActive(!skipTutorialButton.activeSelf);
            Debug.Log($"Skip button toggled: {skipTutorialButton.activeSelf}");
        }
        else
        {
            Debug.LogWarning("Skip button reference is null!");
        }
    }
    #endregion
}