using System;
using System.Collections.Generic;
using UnityEngine;

public enum WallState { Standing = 0, Broken = 1 }

public class WallManager : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 1;
    [SerializeField] private int damagePerHit = 1;

    [Header("Prefabs by State (0 = Standing, 1 = Broken)")]
    [SerializeField] private GameObject[] prefabs;

    [Header("Damage Filter")]
    [SerializeField] private bool onlyMinotaurCanBreak = true;//only damage from Minotaur will be accepted.

    private int _currentHealth;
    private GameObject _spawnedWall;
    private WallState _currentState = WallState.Standing;
    private readonly PillarContext _context = new PillarContext();

    private readonly Dictionary<GameObject, IPillar> _behaviours = new Dictionary<GameObject, IPillar>();

    private void Awake()
    {
        _context.Origin = transform;
        _currentHealth = maxHealth;
        SpawnCurrentState();
    }

    private void OnDestroy()
    {
        DestroySpawned();
    }

    private void SpawnCurrentState()
    {
        if (prefabs == null || prefabs.Length < 2)
        {
            Debug.LogError("[WallManager] Prefabs not set (need 2).");
            return;
        }

        var prefab = prefabs[(int)_currentState];
        if (!prefab)
        {
            Debug.LogError("[WallManager] Missing prefab for state: " + _currentState);
            return;
        }

        _spawnedWall = Instantiate(prefab, transform.position, Quaternion.identity, this.transform);
        if (_spawnedWall.TryGetComponent<IPillar>(out var behaviour))
        {
            if (_spawnedWall.TryGetComponent<Wall_Standing>(out var standing))
            {
                standing.ConfigureDamageFilter(onlyMinotaurCanBreak);
            }

            behaviour.SpawnPillar(_context);
            behaviour.OnDamaged += ReceiveDamage;
            _behaviours[_spawnedWall] = behaviour;
        }
        else
        {
            Debug.LogError("[WallManager] Spawned prefab does not implement IPillar.");
        }
    }

    private void DestroySpawned()
    {
        if (_spawnedWall && _behaviours.TryGetValue(_spawnedWall, out var behaviour))
        {
            behaviour.DestroyPillar(_context);
            behaviour.OnDamaged -= ReceiveDamage;
            _behaviours.Remove(_spawnedWall);
        }
    }

    private void ReceiveDamage()
    {
        if (_currentState == WallState.Broken) return;

        _currentHealth -= Mathf.Max(damagePerHit, 1);
        if (_currentHealth <= 0)
        {
            ToBroken();
        }
    }

    private void ToBroken()
    {
        DestroySpawned();
        _currentState = WallState.Broken;
        _currentHealth = maxHealth;
        SpawnCurrentState();
    }
}
