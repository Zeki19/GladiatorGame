using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial Task / New Task", fileName = "Segment")]

public abstract class TutorialMission : ScriptableObject
{
    [Header("Mission Configuration")]
    public string missionName;
    public EnumDialogues dialogueToPlay = EnumDialogues.None;

    [Header("Camera Settings")]
    public GameObject cameraTarget;
    public float cameraMoveDuration = 2f;

    [Header("UI Settings")]
    public bool showUIHint = false;
    public GameObject uiHintPrefab;

    protected bool _isCompleted = false;
    protected TutorialManager _manager;

    public virtual void Initialize(TutorialManager manager)
    {
        _manager = manager;
        _isCompleted = false;
        OnInitialize();
    }

    public virtual void Cleanup()
    {
        OnCleanup();
    }

    public bool IsCompleted()
    {
        return _isCompleted;
    }

    public void ForceComplete()
    {
        _isCompleted = true;
    }

    protected abstract void OnInitialize();
    protected abstract void OnCleanup();
}