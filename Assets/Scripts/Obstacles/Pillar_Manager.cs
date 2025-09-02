using System;
using System.Collections.Generic;
using UnityEngine;

public enum PillarState { Standing = 0, Fallen = 1, Shattered = 2 }

public class PillarManager : MonoBehaviour
{
    public GameObject[] prefabs;

    private GameObject _spawnedPillar;
    private PillarState _currentState = PillarState.Standing;
    private readonly PillarContext _context = new PillarContext();

    private void Start()
    {
        _context.Origin = transform;
        SpawnCurrent();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) NextState();
    }

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
            behaviour.StartSpawn(_context);
        }
    }
}
