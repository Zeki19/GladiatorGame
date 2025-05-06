using System;
using Enemies;
using Entities;
using UnityEngine;

public class EnemyManager : EntityManager
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private int maxLife;

    private void Awake()
    {
        HealthSystem = new HealthSystem.HealthSystem(maxLife);
        HealthSystem.OnDamage += PrintHealth;
        HealthSystem.OnHeal += PrintHealth;
    }

    private void Start()
    {
        ServiceLocator.Instance.GetService<EnemiesManager>().RegisterEnemy(gameObject,this);
    }

    private void Update()
    {
    }
    private void PrintHealth(float ignore)
    {
        Debug.Log(HealthSystem.currentHealth);
    }
    public Vector2 GetEnemyPosition() 
    { 
        return enemy.transform.position;
    }

    public IHealth GetEnemyHealthSystem()
    {
        return HealthSystem;
    }
}
