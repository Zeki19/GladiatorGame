using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Prefabs")]
    [SerializeField] private GameObject playerHealthbar;

    [Header("Transforms")]
    [SerializeField] private Transform pHCanvas;


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
        StartCoroutine(WaitAndSpawn());
    }

    private IEnumerator WaitAndSpawn()
    {
        yield return new WaitUntil(() => ServiceLocator.Instance.GetService<PlayerManager>() != null);
        SpawnPlayerHealthbar();
    }


    //Debugged with Chat GPT
    private void SpawnPlayerHealthbar()
    {
        // 1) Prefab and are in Inspector
        if (playerHealthbar == null)
        {
            Debug.LogError("[UIManager]  prefab 'playerHealthbar' not assigned in the Inspector.");
            return;
        }
        if (pHCanvas == null)
        {
            Debug.LogError("[UIManager] Transform 'pHCanvas'  not assigned in the Inspector.");
            return;
        }

        // 2) PlayerManager
        var playerManager = ServiceLocator.Instance.GetService<PlayerManager>();
        if (playerManager == null)
        {
            Debug.LogError("[UIManager] ServiceLocator returned  NULL for PlayerManager.");
            return;
        }

        // 3) make sure HealthComponent not null
        var healthComponent = playerManager.HealthComponent;
        if (healthComponent == null)
        {
            Debug.LogError("[UIManager] HealthComponent is NULL in PlayerManager.");
            return;
        }

        // 4) intantiate bar
        GameObject healthbarGO = Instantiate(playerHealthbar, pHCanvas);
        if (healthbarGO == null)
        {
            Debug.LogError("[UIManager] Instantiate returned NULL (is prefaba a valid gameObject?).");
            return;
        }

        var healthbar = healthbarGO.GetComponent<Player_Healthbar>();
        if (healthbar == null)
        {
            Debug.LogError("[UIManager] prefab 'playerHealthbar' Dosent have Player_Healthbar.");
            return;
        }

        // 5) Setup
        healthbar.Setup(healthComponent);
    }
}