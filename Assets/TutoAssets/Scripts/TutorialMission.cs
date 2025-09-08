using UnityEngine;
using System;

[System.Serializable]
public abstract class TutorialMission : ScriptableObject
{
    [Header("Mission Configuration")]
    public string missionName;
    public string missionDescription;
    public EnumDialogues dialogueToPlay = EnumDialogues.None;

    [Header("Camera Settings")]
    public bool shouldMoveCamera = false;
    public GameObject cameraTarget;
    public float cameraMoveDuration = 2f;
    public bool zoomIn = false;
    public float zoomAmount = 5f;

    [Header("UI Settings")]
    public bool showUIHint = false;
    public GameObject uiHintPrefab;
    public Vector2 hintPosition = Vector2.zero;

    [Header("Completion Settings")]
    public float autoCompleteDelay = -1f; 

    protected bool _isCompleted = false;
    protected TutorialManager _manager;

    public event Action OnMissionStart;
    public event Action OnMissionComplete;

    public virtual void Initialize(TutorialManager manager)
    {
        _manager = manager;
        _isCompleted = false;
        OnMissionStart?.Invoke();
        OnInitialize();
    }

    public virtual void UpdateMission()
    {
        if (!_isCompleted)
        {
            CheckCompletion();
        }
    }

    public virtual void Cleanup()
    {
        OnCleanup();
        OnMissionComplete?.Invoke();
        OnMissionStart = null;
        OnMissionComplete = null;
    }

    public bool IsCompleted() => _isCompleted;

    protected void CompleteMission()
    {
        if (!_isCompleted)
        {
            _isCompleted = true;
            Debug.Log($"Mission '{missionName}' marked as complete");
        }
    }

    public void ForceComplete()
    {
        CompleteMission();
    }

    protected abstract void OnInitialize();
    protected abstract void CheckCompletion();
    protected abstract void OnCleanup();
}
