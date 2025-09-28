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
    [SerializeField] private bool isTrainingMode = false; // Bool para controlar si es modo entrenamiento

    [Header("Dependencies")]
    private DialogueManager _dialogueManager;
    private CameraTutorialManager _cameraTutorialManager;

    private TutorialMission _currentMission;
    private int _currentMissionIndex = 0;
    private TutorialState _currentState = TutorialState.NotStarted;
    private bool _dialogueStarted = false;
    private bool _tutorialCompleted = false; // Para saber si el tutorial se completó

    public static event Action<TutorialMission> OnMissionStarted;
    public static event Action<TutorialMission> OnMissionCompleted;
    public static event Action OnTutorialCompleted;
    public static event Action OnTutorialRestart; // Nuevo evento para reiniciar tutorial

    // Propiedades públicas para acceder desde otros scripts
    public bool IsTrainingMode => isTrainingMode;
    public bool IsTutorialCompleted => _tutorialCompleted;

    private void Awake()
    {
        ServiceLocator.Instance.RegisterService(this);
    }

    private void Start()
    {
        InitializeDependencies();

        // Solo iniciar tutorial si no estamos en modo entrenamiento
        if (!isTrainingMode)
        {
            StartTutorial();
        }
        else
        {
            // En modo entrenamiento, mostrar mensaje de exploración libre
            if (missionDescriptionUI != null)
            {
                missionDescriptionUI.text = "Training Mode - Explore freely or press E near the dummy to restart tutorial";
            }
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

    // Método público para activar/desactivar modo entrenamiento
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

    // Método público para reiniciar el tutorial desde 0
    public void RestartTutorial()
    {
        // Solo permitir reinicio si el tutorial se completó o estamos en modo entrenamiento
        if (!_tutorialCompleted && !isTrainingMode)
            return;

        Debug.Log("Restarting tutorial from beginning...");

        // Reiniciar variables
        _currentMissionIndex = 0;
        _currentMission = null;
        _currentState = TutorialState.NotStarted;
        _dialogueStarted = false;
        _tutorialCompleted = false;
        isTrainingMode = false;

        // Limpiar UI hint si existe
        HideUIHint();

        // Disparar evento de reinicio
        OnTutorialRestart?.Invoke();

        // Iniciar tutorial
        StartTutorial();
    }

    private void StartTutorial()
    {
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

        Debug.Log($"Starting Mission: {_currentMission.missionName}");

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

        // Auto-reset camera after some events
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
        _tutorialCompleted = true; // Marcar como completado

        // Cambiar el texto de la UI AQUÍ, cuando se completa el tutorial
        if (missionDescriptionUI != null)
        {
            missionDescriptionUI.text = "Train or face your opponent";
        }

        OnTutorialCompleted?.Invoke();

        // Hacer zoom a la puerta de salida antes del diálogo final
        if (_cameraTutorialManager != null)
        {
            StartCoroutine(CompleteTutorialWithCameraZoom());
        }
        else if (_dialogueManager != null)
        {
            _dialogueManager.StartConversation(EnumDialogues.Mission7);
        }
    }

    private IEnumerator CompleteTutorialWithCameraZoom()
    {
        if (_dialogueManager != null)
        {
            _dialogueManager.StartConversation(EnumDialogues.Mission7);

            bool dialogueComplete = false;
            _dialogueManager.OnConversationEnd = () => dialogueComplete = true;
            yield return new WaitUntil(() => dialogueComplete);
        }
        CameraEventConfig exitCameraEvent = new CameraEventConfig
        {
            eventId = "Exit",
            targetTag = "Exit",
            targetName = "ExitDoor",
            moveDuration = 2.5f,
            shouldZoom = true,
            zoomAmount = 6f,
            zoomDuration = 1.5f
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

    [ContextMenu("Force Complete Current Mission")]
    public void ForceCompleteCurrentMission()
    {
        if (_currentMission != null && _currentState == TutorialState.WaitingForCompletion)
        {
            _currentMission.ForceComplete();
        }
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
}