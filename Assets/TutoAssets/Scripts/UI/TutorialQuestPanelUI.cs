using UnityEngine;
using TMPro;

public class TutorialQuestPanelUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject questEntryPrefab;
    [SerializeField] private Transform questContainer;
    [SerializeField] private GameObject panelRoot;

    [Header("Completion Quest Settings")]
    [SerializeField] private string completionQuestLabel = "Continue your journey";
    [SerializeField] private string completionQuestDescription = "Train or face your opponent\nPress E near dummy to restart tutorial";

    private TutorialManager _tutorialManager;
    private TutorialQuestEntryUI _currentQuestEntry;

    private void Awake()
    {
        if (panelRoot != null)
        {
            panelRoot.SetActive(true);
        }
    }

    private void Start()
    {
        _tutorialManager = ServiceLocator.Instance.GetService<TutorialManager>();

        if (_tutorialManager == null)
        {
            Debug.LogError("TutorialQuestPanelUI: TutorialManager not found in ServiceLocator!");
            enabled = false;
            return;
        }

        TutorialManager.OnMissionStarted += OnMissionStarted;
        TutorialManager.OnMissionCompleted += OnMissionCompleted;
        TutorialManager.OnTutorialCompleted += OnTutorialCompleted;
        TutorialManager.OnTutorialRestart += OnTutorialRestart;

        if (_tutorialManager.IsTrainingMode || _tutorialManager.IsTutorialCompleted)
        {
            ShowCompletionQuest();
        }
        else
        {
            CheckForActiveMission();
        }
    }

    private void OnDestroy()
    {
        TutorialManager.OnMissionStarted -= OnMissionStarted;
        TutorialManager.OnMissionCompleted -= OnMissionCompleted;
        TutorialManager.OnTutorialCompleted -= OnTutorialCompleted;
        TutorialManager.OnTutorialRestart -= OnTutorialRestart;
    }

    private void CheckForActiveMission()
    {
        StartCoroutine(CheckForActiveMissionDelayed());
    }

    private System.Collections.IEnumerator CheckForActiveMissionDelayed()
    {
        yield return null;
        yield return null;

        if (_currentQuestEntry == null && !_tutorialManager.IsTrainingMode && !_tutorialManager.IsTutorialCompleted)
        {
            Debug.Log("TutorialQuestPanelUI: No quest detected, forcing first mission display...");

            TutorialMission currentMission = _tutorialManager.GetCurrentMission();

            if (currentMission != null)
            {
                Debug.Log($"TutorialQuestPanelUI: Forcing display of mission: {currentMission.missionName}");
                SpawnQuestEntry(currentMission.missionName, currentMission.missionDescription);
            }
            else
            {
                var missions = _tutorialManager.GetAllMissions();
                if (missions != null && missions.Count > 0)
                {
                    Debug.Log($"TutorialQuestPanelUI: Forcing display of first mission: {missions[0].missionName}");
                    SpawnQuestEntry(missions[0].missionName, missions[0].missionDescription);
                }
                else
                {
                    Debug.LogWarning("TutorialQuestPanelUI: No missions available to display!");
                }
            }
        }
    }


    private void OnMissionStarted(TutorialMission mission)
    {
        Debug.Log($"TutorialQuestPanelUI: OnMissionStarted - {mission.missionName}");

        if (panelRoot != null && !_tutorialManager.IsTrainingMode)
        {
            panelRoot.SetActive(true);
        }

        if (_currentQuestEntry != null)
        {
            StartCoroutine(TransitionToNewQuest(mission.missionName, mission.missionDescription));
        }
        else
        {
            SpawnQuestEntry(mission.missionName, mission.missionDescription);
        }
    }

    private System.Collections.IEnumerator TransitionToNewQuest(string newLabel, string newDescription)
    {
        bool fadeOutComplete = false;

        _currentQuestEntry.OnFadeOutComplete += () => fadeOutComplete = true;

        _currentQuestEntry.FadeOutAndDestroy();
        _currentQuestEntry = null;

        yield return new WaitUntil(() => fadeOutComplete);

        yield return new WaitForSeconds(0.1f);

        SpawnQuestEntry(newLabel, newDescription);
    }


    private void OnMissionCompleted(TutorialMission mission)
    {
        Debug.Log($"TutorialQuestPanelUI: Mission '{mission.missionName}' completed.");
    }


    private void OnTutorialCompleted()
    {
        Debug.Log("TutorialQuestPanelUI: Tutorial completed - showing completion quest");

        if (_currentQuestEntry != null)
        {
            StartCoroutine(TransitionToCompletionQuest());
        }
        else
        {
            ShowCompletionQuest();
        }
    }


    private System.Collections.IEnumerator TransitionToCompletionQuest()
    {
        bool fadeOutComplete = false;

        _currentQuestEntry.OnFadeOutComplete += () => fadeOutComplete = true;

        _currentQuestEntry.FadeOutAndDestroy();
        _currentQuestEntry = null;

        yield return new WaitUntil(() => fadeOutComplete);

        yield return new WaitForSeconds(0.1f);

        ShowCompletionQuest();
    }

    private void OnTutorialRestart()
    {
        Debug.Log("TutorialQuestPanelUI: Tutorial restarted");

        // Clear current quest entry
        if (_currentQuestEntry != null)
        {
            Destroy(_currentQuestEntry.gameObject);
            _currentQuestEntry = null;
        }

        if (panelRoot != null)
        {
            panelRoot.SetActive(true);
        }
    }

    private void ShowCompletionQuest()
    {
        if (panelRoot != null)
        {
            panelRoot.SetActive(true);
        }

        if (_currentQuestEntry != null)
        {
            Destroy(_currentQuestEntry.gameObject);
            _currentQuestEntry = null;
        }

        SpawnQuestEntry(completionQuestLabel, completionQuestDescription);
    }
    private void SpawnQuestEntry(string label, string description)
    {
        if (questEntryPrefab == null)
        {
            Debug.LogError("TutorialQuestPanelUI: questEntryPrefab is not assigned!");
            return;
        }

        if (questContainer == null)
        {
            Debug.LogError("TutorialQuestPanelUI: questContainer is not assigned!");
            return;
        }

        GameObject entryObj = Instantiate(questEntryPrefab, questContainer);
        _currentQuestEntry = entryObj.GetComponent<TutorialQuestEntryUI>();

        if (_currentQuestEntry == null)
        {
            Debug.LogError("TutorialQuestPanelUI: questEntryPrefab does not have TutorialQuestEntryUI component!");
            Destroy(entryObj);
            return;
        }

        _currentQuestEntry.SetQuestData(label, description);

        Debug.Log($"TutorialQuestPanelUI: Spawned quest entry - {label}");
    }

    #region Editor Utilities
    [ContextMenu("Test Show Completion Quest")]
    private void TestShowCompletionQuest()
    {
        ShowCompletionQuest();
    }

    [ContextMenu("Test Restart")]
    private void TestRestart()
    {
        OnTutorialRestart();
    }

    [ContextMenu("Clear Current Quest")]
    private void ClearCurrentQuest()
    {
        if (_currentQuestEntry != null)
        {
            Destroy(_currentQuestEntry.gameObject);
            _currentQuestEntry = null;
        }
    }
    #endregion
}