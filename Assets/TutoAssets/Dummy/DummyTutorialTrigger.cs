using UnityEngine;

public class DummyTutorialTrigger : MonoBehaviour
{
    [Header("UI Configuration")]
    [SerializeField] private GameObject interactionUI;
    [SerializeField] private KeyCode interactionKey = KeyCode.E;

    private bool _playerInTrigger = false;
    private TutorialManager _tutorialManager;

    private void Start()
    {
        _tutorialManager = ServiceLocator.Instance.GetService<TutorialManager>();

        if (interactionUI != null)
        {
            interactionUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (_playerInTrigger && Input.GetKeyDown(interactionKey))
        {
            if (CanRestartTutorial())
            {
                RestartTutorial();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInTrigger = true;

            if (CanRestartTutorial())
            {
                ShowInteractionUI();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInTrigger = false;
            HideInteractionUI();
        }
    }

    private bool CanRestartTutorial()
    {
        if (_tutorialManager == null) return false;
        return _tutorialManager.IsTutorialCompleted || _tutorialManager.IsTrainingMode;
    }

    private void RestartTutorial()
    {
        if (_tutorialManager != null)
        {
            Debug.Log("Player activated tutorial restart from dummy trigger");
            HideInteractionUI();
            _tutorialManager.RestartTutorial();
        }
    }

    private void ShowInteractionUI()
    {
        if (interactionUI != null)
        {
            interactionUI.SetActive(true);
        }
    }

    private void HideInteractionUI()
    {
        if (interactionUI != null)
        {
            interactionUI.SetActive(false);
        }
    }

    // Método para configurar el UI desde el inspector o código
    public void SetInteractionUI(GameObject ui)
    {
        interactionUI = ui;
    }
}