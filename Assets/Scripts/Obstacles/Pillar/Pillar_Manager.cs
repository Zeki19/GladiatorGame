using System;
using System.Collections.Generic;
using UnityEngine;

public enum PillarState { Standing = 0, Fallen = 1, Shattered = 2}

public class PillarManager : MonoBehaviour
{
    [SerializeField] private int health;
    private int _currentHealth;
    [SerializeField] private GameObject[] prefabs;

    private GameObject _spawnedPillar;
    private PillarState _currentState = PillarState.Standing;
    private readonly PillarContext _context = new PillarContext();
    
    private Dictionary<GameObject, IPillar>  _pillars = new Dictionary<GameObject, IPillar>();
    
    private SpriteRenderer _renderer;
    [SerializeField] private List<Collider2D> ignoreColliders;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.enabled = false;
        
        _context.Origin = transform;
        
        _currentHealth = health;
        
        SpawnCurrentPillar();
    }

    private void OnDamage()
    {
        _currentHealth--;
        
        if (_currentHealth > 0) return;
        NextState();
    }

    private void NextState()
    {
        DestroySpawnedPillar();
        _currentState = (PillarState)(((int)_currentState + 1) % 3);
        SpawnCurrentPillar();
        ServiceLocator.Instance.GetService<NavMeshService>().RebuildNavMesh();
        _currentHealth = health;
    }

    private void DestroySpawnedPillar()
    {
        if (_spawnedPillar && _pillars.TryGetValue(_spawnedPillar, out var behaviour))
        {
            behaviour.DestroyPillar(_context);
            _pillars.Remove(_spawnedPillar);
        }
    }
    
    private void SpawnCurrentPillar()
    {
        if (_spawnedPillar) Destroy(_spawnedPillar);

        var prefab = prefabs[(int)_currentState];
        if (!prefab)
        {
            Debug.LogWarning("Missing prefab for state " + _currentState);
            return;
        }

        _spawnedPillar = Instantiate(prefab, transform.position, Quaternion.identity, this.gameObject.transform);

        if (_spawnedPillar.TryGetComponent<IPillar>(out var behaviour))
        {
            behaviour.SpawnPillar(_context);

            behaviour.OnDamaged += OnDamage;

            _pillars.TryAdd(_spawnedPillar, behaviour);
            behaviour.AddIgnorePillar(ignoreColliders);
        }
    }
}
