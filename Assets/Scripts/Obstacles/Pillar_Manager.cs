using System;
using System.Collections.Generic;
using UnityEngine;

public enum PillarState { Standing = 0, Fallen = 1, Shattered = 2 }

public class PillarManager : MonoBehaviour
{
    [SerializeField] private int maxLife;
    protected IHealth HealthSystem;
    public IHealth HealthComponent => HealthSystem;

    public GameObject[] prefabs;

    private GameObject _spawnedPillar;
    private PillarState _currentState = PillarState.Standing;
    private readonly PillarContext _context = new PillarContext();

    protected virtual void Start()
    {
        HealthSystem = new HealthSystem.HealthSystem(maxLife);
        _context.Origin = transform;
        HealthSystem.OnDead += OnPillarDeath;

        if (TryGetComponent<IPillar>(out var behaviour))
            behaviour.StartSpawn(_context);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            NextState();
            Debug.Log($"[DEBUG] Pilar cambiado a estado: {_currentState}");
        }

        if ((Input.GetKeyDown(KeyCode.N)))
        {
            float damageAmount = 25;
            Debug.Log($"[DEBUG] Haciendo {damageAmount} de daño al pilar en estado {_currentState}");

            HealthSystem.TakeDamage(damageAmount);

            Debug.Log($"[DEBUG] Vida actual: {HealthSystem.GetCurrentHealth()} / {HealthSystem.GetMaxHealth()}");

            if (!HealthSystem.IsAlive())
            {
                Debug.Log($"[DEBUG] Pilar muerto en estado {_currentState}");
            }
        }
    }

    //if (Input.GetMouseButtonDown(1)) NextState();
    //    if (Input.GetMouseButtonDown(3)) HealthSystem.TakeDamage(25);

    private void NextState()
    {
        _currentState = (PillarState)(((int)_currentState + 1) % 3);
        SpawnCurrent();
    }
    

    private void SpawnCurrent()
    {
        if (_spawnedPillar) Destroy(_spawnedPillar);

        var prefab = prefabs[(int)_currentState];
        if (!prefab) { Debug.LogWarning("Missing prefab for state " + _currentState); return; }

        _spawnedPillar = Instantiate(prefab, transform.position, Quaternion.identity);

        if (_spawnedPillar.TryGetComponent<IPillar>(out var behaviour))
        {
            behaviour.StartSpawn(_context, HealthSystem);
        }
    }

    private void OnPillarDeath()
    {
        NextState(); 
    }
}

