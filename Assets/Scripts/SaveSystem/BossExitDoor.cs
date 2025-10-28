using UnityEngine;

public class BossExitDoor : MonoBehaviour
{
    [Header("Boss Configuration")]
    [SerializeField] private int bossNumber = 1;
    [SerializeField] private int tutorialSceneIndex = 1;
    
    [Header("Victory Scene Configuration")]
    [Tooltip("Scene name or index to load after defeating the final boss (Boss 3)")]
    [SerializeField] private string victorySceneName = "VictoryScene";
    [SerializeField] private bool useSceneIndex = false;
    [SerializeField] private int victorySceneIndex = 5;

    [Header("Door Sprites")]
    [SerializeField] private SpriteRenderer doorSpriteRenderer;
    [SerializeField] private Sprite closedDoorSprite;
    [SerializeField] private Sprite openDoorSprite;

    [Header("Debug/Testing")]
    [SerializeField] private bool skipBossDefeatCheck = false;

    private bool _bossDefeated = false;

    private void Start()
    {
        ServiceLocator.Instance.RegisterService(this);
        UpdateDoorSprite();
    }

    public void OnBossDefeated()
    {
        _bossDefeated = true;
        UpdateDoorSprite();
        Debug.Log($"Boss {bossNumber} defeated! Door is now open.");
    }

    private void UpdateDoorSprite()
    {
        if (doorSpriteRenderer == null) return;

        bool isDoorOpen = skipBossDefeatCheck || _bossDefeated;
        doorSpriteRenderer.sprite = isDoorOpen ? openDoorSprite : closedDoorSprite;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!skipBossDefeatCheck && !_bossDefeated)
            {
                Debug.Log("Boss not defeated yet! Defeat the boss first.");
                return;
            }

            HandleBossCompletion();
        }
    }

    private void HandleBossCompletion()
    {
        if (bossNumber == 3)
        {
            HandleFinalBossVictory();
        }
        else
        {
            HandleRegularBossVictory();
        }
    }

    private void HandleFinalBossVictory()
    {
        Debug.Log("=== FINAL BOSS DEFEATED ===");
        Debug.Log("Going to Victory Scene and resetting save to tutorial state...");

        SaveManager.Instance.StartNewGame();
        
        SceneChanger sceneChanger = ServiceLocator.Instance.GetService<SceneChanger>();
        
        if (useSceneIndex)
        {
            sceneChanger.ChangeScene(victorySceneIndex);
            Debug.Log($"Loading Victory Scene by index: {victorySceneIndex}");
        }
        else
        {
            sceneChanger.ChangeScene(victorySceneName);
            Debug.Log($"Loading Victory Scene by name: {victorySceneName}");
        }
    }

    private void HandleRegularBossVictory()
    {
        Debug.Log($"Boss {bossNumber} defeated! Returning to training hub.");
        
        SaveManager.Instance.SaveAfterBossDefeat(bossNumber);
        
        ServiceLocator.Instance.GetService<SceneChanger>().ChangeScene(tutorialSceneIndex);
    }

    [ContextMenu("Simulate Boss Defeat")]
    public void SimulateBossDefeat()
    {
        OnBossDefeated();
    }

    [ContextMenu("Test Final Boss Victory")]
    public void TestFinalBossVictory()
    {
        Debug.Log("=== TESTING FINAL BOSS VICTORY ===");
        _bossDefeated = true;
        UpdateDoorSprite();
        
        int originalBossNumber = bossNumber;
        bossNumber = 3;
        
        HandleBossCompletion();
        
        bossNumber = originalBossNumber;
    }

    private void OnValidate()
    {
        UpdateDoorSprite();
    }
}