using System;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Header("Tutorial Configuration")]
    [SerializeField] private List<TutorialMission> missions = new List<TutorialMission>();
    [SerializeField] private int currentMissionIndex = 0;

    [Header("References")]
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private CameraHelper cameraHelper;

    private TutorialMission _currentMission;
    private bool _isProcessingMission = false;

    public static event Action<TutorialMission> OnMissionStarted;
    public static event Action<TutorialMission> OnMissionCompleted;
    public static event Action OnTutorialCompleted;

    private void Awake()
    {
        ServiceLocator.Instance.RegisterService(this);
    }

    private void Start()
    {
        if (dialogueManager == null)
            dialogueManager = ServiceLocator.Instance.GetService<DialogueManager>();

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

        currentMissionIndex = index;
        _currentMission = missions[currentMissionIndex];
        _isProcessingMission = true;

        Debug.Log($"Starting Mission: {_currentMission.missionName}");
        OnMissionStarted?.Invoke(_currentMission);

        StartCoroutine(ProcessMission(_currentMission));
    }

    private IEnumerator ProcessMission(TutorialMission mission)
    {
        // Initialize mission
        mission.Initialize(this);

        // Start dialogue if configured
        if (mission.dialogueToPlay != EnumDialogues.None)
        {
            bool dialogueComplete = false;
            dialogueManager.OnConversationEnd = () => dialogueComplete = true;
            dialogueManager.StartConversation(mission.dialogueToPlay);

            yield return new WaitUntil(() => dialogueComplete);
        }

        // Handle camera movement if configured
        if (mission.cameraTarget != null)
        {
            if (cameraHelper != null)
            {
                cameraHelper.MoveToTarget(mission.cameraTarget.transform);
                yield return new WaitForSeconds(mission.cameraMoveDuration);
            }
        }

        // Show UI hints if configured
        if (mission.showUIHint)
        {
            ShowUIHint(mission.uiHintPrefab);
        }

        // Wait for mission completion
        yield return new WaitUntil(() => mission.IsCompleted());

        // Hide UI hints
        if (mission.showUIHint)
        {
            HideUIHint();
        }

        // Mission completed
        CompleteMission();
    }

    private void CompleteMission()
    {
        Debug.Log($"Mission Completed: {_currentMission.missionName}");

        OnMissionCompleted?.Invoke(_currentMission);
        _currentMission.Cleanup();
        _isProcessingMission = false;
        StartMission(currentMissionIndex + 1);
    }

    private void CompleteTutorial()
    {
        Debug.Log("Tutorial Completed!");
        OnTutorialCompleted?.Invoke();

        // Open exit door or trigger next scene
        if (dialogueManager != null)
        {
            dialogueManager.StartConversation(EnumDialogues.Mission7);
        }
    }

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

    [ContextMenu("Force Complete Current Mission")]
    public void ForceCompleteCurrentMission()
    {
        if (_currentMission != null)
        {
            _currentMission.ForceComplete();
        }
    }
}