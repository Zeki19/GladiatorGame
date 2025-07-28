using System;
using Enemies;
using Entities;
using UnityEngine;

public class EnemyManager : EntityManager
{
    [SerializeField] private int maxLife;

    private void Awake()
    {
        HealthSystem = new HealthSystem.HealthSystem(maxLife);
        HealthSystem.OnDamage += PrintHealth;
        HealthSystem.OnHeal += PrintHealth;
        ServiceLocator.Instance.RegisterService(this);
    }

    private void Start()
    {
        ServiceLocator.Instance.GetService<EnemiesManager>().RegisterEnemy(gameObject,this);
    }
    private void PrintHealth(float ignore)
    {
        Debug.Log(HealthSystem.currentHealth);
    }
    public Vector2 GetEnemyPosition() 
    { 
        return gameObject.transform.position;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var status = controller as IStatus;
        if (other.gameObject.CompareTag("Wall") && status != null && status.GetStatus(StatusEnum.Dashing))
        {
            status.SetStatus(StatusEnum.Dashing,false);
        }
    }
}
