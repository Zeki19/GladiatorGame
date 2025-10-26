using UnityEngine;

public class ExitTutorialDoor : MonoBehaviour
{
    [Header("Door Configuration")]
    [SerializeField] private bool isTrainingHubDoor = false;

    [Header("Door Sprites")]
    [SerializeField] private SpriteRenderer doorSpriteRenderer;
    [SerializeField] private Sprite closedDoorSprite;
    [SerializeField] private Sprite openDoorSprite;

    private TutorialManager _tutorialManager;
    private bool _playerInRange = false;

    private void Start()
    {
        _tutorialManager = ServiceLocator.Instance.GetService<TutorialManager>();

        // Inicializar sprite
        UpdateDoorSprite();
    }

    private void Update()
    {
        // Actualizar sprite en tiempo real según el estado del tutorial
        UpdateDoorSprite();
    }

    private void UpdateDoorSprite()
    {
        if (doorSpriteRenderer == null) return;

        bool isDoorOpen = IsDoorOpen();
        doorSpriteRenderer.sprite = isDoorOpen ? openDoorSprite : closedDoorSprite;
    }

    private bool IsDoorOpen()
    {
        if (isTrainingHubDoor)
        {
            // Training Hub door siempre está abierta
            return true;
        }

        // Tutorial door se abre cuando el tutorial está completo
        return _tutorialManager != null && _tutorialManager.IsTutorialCompleted;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = true;

            if (_tutorialManager != null && !_tutorialManager.IsTutorialCompleted)
            {
                Debug.Log("Tutorial not completed yet! Complete the tutorial first.");
                return;
            }

            LoadNextScene();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = false;
        }
    }

    private void LoadNextScene()
    {
        SaveData saveData = SaveManager.Instance.GetCurrentSaveData();

        if (isTrainingHubDoor)
        {
            int nextScene = saveData.nextSceneToLoad;
            SaveManager.Instance.SaveBeforeLeavingTrainingHub(nextScene);
            ServiceLocator.Instance.GetService<SceneChanger>().ChangeScene(nextScene);
            Debug.Log($"Leaving training hub to scene: {nextScene}");
        }
        else
        {
            SaveManager.Instance.SaveAfterTutorialComplete();
            int nextScene = SaveManager.Instance.GetCurrentSaveData().nextSceneToLoad;
            ServiceLocator.Instance.GetService<SceneChanger>().ChangeScene(nextScene);
            Debug.Log($"Tutorial completed! Going to Boss 1 (scene {nextScene})");
        }
    }
}