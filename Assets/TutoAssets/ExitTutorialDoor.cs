using UnityEngine;

public class ExitTutorialDoor : MonoBehaviour
{
    [Header("Door Configuration")]
    [SerializeField] private bool isTrainingHubDoor = false; 

    private TutorialManager _tutorialManager;
    private bool _playerInRange = false;

    private void Start()
    {
        _tutorialManager = ServiceLocator.Instance.GetService<TutorialManager>();
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