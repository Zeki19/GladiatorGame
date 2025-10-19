using UnityEngine;
using TMPro;

/// <summary>
/// Manages the tutorial quest panel UI in the top-left corner.
/// Displays the current active quest and updates dynamically as quests are completed.
/// </summary>
public class TutorialQuestPanelUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject questEntryPrefab;
    [SerializeField] private Transform questContainer;
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private TextMeshProUGUI completionMessageText;

    [Header("Settings")]
    [SerializeField] private string completionMessage = "Tutorial Complete!";
    [SerializeField] private bool hideOnCompletion = false;

    private TutorialManager _tutorialManager;
    private TutorialQuestEntryUI _currentQuestEntry;

    private void Awake()
    {
        // Hide completion message initially
        if (completionMessageText != null)
        {
            completionMessageText.gameObject.SetActive(false);
        }

        // Ensure panel is visible initially
        if (panelRoot != null)
        {
          //  panelRoot.SetActive(false); // Start hidden until tutorial starts
        }
    }

    private void Start()
    {
        // Get TutorialManager from ServiceLocator
        _tutorialManager = ServiceLocator.Instance.GetService<TutorialManager>();

        if (_tutorialManager == null)
        {
            Debug.LogError("TutorialQuestPanelUI: TutorialManager not found in ServiceLocator!");
            enabled = false;
            return;
        }

        // Subscribe to tutorial events
        TutorialManager.OnMissionStarted += OnMissionStarted;
        TutorialManager.OnMissionCompleted += OnMissionCompleted;
        TutorialManager.OnTutorialCompleted += OnTutorialCompleted;
        TutorialManager.OnTutorialRestart += OnTutorialRestart;

        // Check if tutorial is already in progress or in training mode
        if (_tutorialManager.IsTrainingMode || _tutorialManager.IsTutorialCompleted)
        {
            if (panelRoot != null)
            {
                panelRoot.SetActive(false);
            }
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from events
        TutorialManager.OnMissionStarted -= OnMissionStarted;
        TutorialManager.OnMissionCompleted -= OnMissionCompleted;
        TutorialManager.OnTutorialCompleted -= OnTutorialCompleted;
        TutorialManager.OnTutorialRestart -= OnTutorialRestart;
    }

    /// <summary>
    /// Called when a new mission starts.
    /// </summary>
    private void OnMissionStarted(TutorialMission mission)
    {
        // Show panel when first mission starts
        if (panelRoot != null && !_tutorialManager.IsTrainingMode)
        {
            panelRoot.SetActive(true);
        }

        // Remove previous quest entry if it exists
        if (_currentQuestEntry != null)
        {
            _currentQuestEntry.FadeOutAndDestroy();
            _currentQuestEntry = null;
        }

        // Create new quest entry
        SpawnQuestEntry(mission);
    }

    /// <summary>
    /// Called when a mission is completed.
    /// </summary>
    private void OnMissionCompleted(TutorialMission mission)
    {
        // The next mission will be started automatically by TutorialManager
        // which will trigger OnMissionStarted
        // We can add additional completion effects here if needed
        Debug.Log($"TutorialQuestPanelUI: Mission '{mission.missionName}' completed.");
    }

    /// <summary>
    /// Called when the entire tutorial is completed.
    /// </summary>
    private void OnTutorialCompleted()
    {
        // Remove current quest entry
        if (_currentQuestEntry != null)
        {
            _currentQuestEntry.FadeOutAndDestroy();
            _currentQuestEntry = null;
        }

        // Show completion message or hide panel
        if (hideOnCompletion)
        {
            if (panelRoot != null)
            {
                panelRoot.SetActive(false);
            }
        }
        else
        {
            if (completionMessageText != null)
            {
                completionMessageText.text = completionMessage;
                completionMessageText.gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Called when the tutorial is restarted.
    /// </summary>
    private void OnTutorialRestart()
    {
        // Hide completion message
        if (completionMessageText != null)
        {
            completionMessageText.gameObject.SetActive(false);
        }

        // Clear current quest entry
        if (_currentQuestEntry != null)
        {
            Destroy(_currentQuestEntry.gameObject);
            _currentQuestEntry = null;
        }

        // Show panel
        if (panelRoot != null)
        {
            panelRoot.SetActive(true);
        }
    }

    /// <summary>
    /// Spawns a new quest entry UI element.
    /// </summary>
    private void SpawnQuestEntry(TutorialMission mission)
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

        // Instantiate the quest entry
        GameObject entryObj = Instantiate(questEntryPrefab, questContainer);
        _currentQuestEntry = entryObj.GetComponent<TutorialQuestEntryUI>();

        if (_currentQuestEntry == null)
        {
            Debug.LogError("TutorialQuestPanelUI: questEntryPrefab does not have TutorialQuestEntryUI component!");
            Destroy(entryObj);
            return;
        }

        // Set the quest data
        _currentQuestEntry.SetQuestData(mission.missionName, mission.missionDescription);
    }

    #region Editor Utilities
    [ContextMenu("Test Show Completion")]
    private void TestShowCompletion()
    {
        OnTutorialCompleted();
    }

    [ContextMenu("Test Restart")]
    private void TestRestart()
    {
        OnTutorialRestart();
    }
    #endregion
}
