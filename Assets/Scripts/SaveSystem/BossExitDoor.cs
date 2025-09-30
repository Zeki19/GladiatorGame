using UnityEngine;

public class BossExitDoor : MonoBehaviour
{
    [Header("Boss Configuration")]
    [SerializeField] private int bossNumber = 1; 
    [SerializeField] private int tutorialSceneIndex = 1; 

    [Header("Debug/Testing")]
    [SerializeField] private bool skipBossDefeatCheck = false; 

    private bool _bossDefeated = false;

    private void Start()
    {
        ServiceLocator.Instance.RegisterService(this);
    }
    public void OnBossDefeated()
    {
        _bossDefeated = true;
        Debug.Log($"Boss {bossNumber} defeated! Door is now open.");

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

            SaveManager.Instance.SaveAfterBossDefeat(bossNumber);

            ServiceLocator.Instance.GetService<SceneChanger>().ChangeScene(tutorialSceneIndex);

            Debug.Log($"Boss {bossNumber} defeated! Returning to training hub.");
        }
    }

    [ContextMenu("Simulate Boss Defeat")]
    public void SimulateBossDefeat()
    {
        OnBossDefeated();
    }
}