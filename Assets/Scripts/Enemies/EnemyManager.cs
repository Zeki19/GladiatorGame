using System;
using Entities;
using UnityEngine;

public class EnemyManager : EntityManager
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private int maxLife;

    private void Awake()
    {
        HealthSystem = new HealthSystem.HealthSystem(maxLife);
    }

    public Vector2 GetEnemyPosition() 
    { 
        return enemy.transform.position;
    }

    public float GetEnemyHealthSystem()
    {
        return 0;
    }
}
