using System;
using Enemies;
using Entities;
using UnityEngine;


public class EnemyManager : EntityManager
{
    [SerializeField] private int maxLife;

    [Header("Boss UI Reference")]
    [SerializeField] private BossHealthBarUI bossHealthBarUI;

    private int currentPhase = 0;

    private void Awake()
    {
        HealthSystem = new HealthSystem.HealthSystem(maxLife);
        HealthSystem.OnDamage += PrintHealth;
        HealthSystem.OnHeal += PrintHealth;
        ServiceLocator.Instance.RegisterService(this);
    }
    private void OnDestroy()
    {
        ServiceLocator.Instance.RemoveService(this);
    }

    protected override void Start()
    {
        base.Start();
        ServiceLocator.Instance.GetService<EnemiesManager>().RegisterEnemy(gameObject, this);
        UpdateBossPhaseUI();
    }

    private void PrintHealth(float ignore)
    {
        Debug.Log(HealthSystem.currentHealth);
    }

    public Vector2 GetEnemyPosition()
    {
        return gameObject.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("olle");
        var status = controller as IStatus;
        if (other.gameObject.CompareTag("Wall") && status != null && status.GetStatus(StatusEnum.Dashing))
        {
            status.SetStatus(StatusEnum.Dashing,false);
        }
    }

    public int CurrentPhase
    {
        get => currentPhase;
        set
        {
            if (currentPhase != value)
            {
                currentPhase = value;
                UpdateBossPhaseUI();
            }
        }
    }

    private void UpdateBossPhaseUI()
    {
        if (bossHealthBarUI != null)
        {
            bossHealthBarUI.SetBossPhase(currentPhase);
        }
    }

    public void AdvancePhase()
    {
        CurrentPhase++;
    }
}