using UnityEngine;

public class BossExitDoor : MonoBehaviour
{
    [Header("Boss Configuration")]
    [SerializeField] private int bossNumber = 1;
    [SerializeField] private int tutorialSceneIndex = 1;

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

        // Inicializar sprite cerrado
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

    // Para actualizar en el editor cuando cambias skipBossDefeatCheck
    private void OnValidate()
    {
        UpdateDoorSprite();
    }
}