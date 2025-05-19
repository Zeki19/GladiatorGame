using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Prefabs")]
    [SerializeField] private GameObject playerHealthbarPrefab;

    [Header("Transforms")]
    [SerializeField] private Transform uiCanvas;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        ServiceLocator.Instance.RegisterService(this);
    }

    private void Start()
    {
        SpawnPlayerHealthbar();
    }

    private void SpawnPlayerHealthbar()
    {
        var playerManager = ServiceLocator.Instance.GetService<PlayerManager>();

        GameObject healthbarGO = Instantiate(playerHealthbarPrefab, uiCanvas);
        Player_Healthbar healthbar = healthbarGO.GetComponent<Player_Healthbar>();
        healthbar.Setup(playerManager.HealthComponent);
    }
}