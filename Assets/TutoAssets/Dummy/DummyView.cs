using System;
using Enemies;
using Entities.Interfaces;
using Unity.Mathematics;
using UnityEngine;

public class DummyView : EnemyView, ILook
{
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Hit = Animator.StringToHash("Hit");
    private static readonly int Death = Animator.StringToHash("Death");

    public GameObject art;

    public override void LookDir(Vector2 dir)
    {
    }

    public override void LookDirInsta(Vector2 dir)
    {
    }

    public override void PlayStateAnimation(StateEnum state)
    {
        switch (state)
        {
            case StateEnum.Idle:
                animator.SetTrigger(Idle);
                break;
            default:     
                animator.SetTrigger(Idle);
                break;
        }
    }

    public override void StopStateAnimation(StateEnum state)
    {
    }

    private void Start()
    {
        base.Start();
        if (manager != null && manager.HealthComponent != null)
        {
            manager.HealthComponent.OnDamage += OnTakeDamage;
            manager.HealthComponent.OnDead += OnDeath;
        }
    }

    private void OnTakeDamage(float damage)
    {
        if (animator != null)
        {
            animator.SetTrigger(Hit);
        }
    }

    private void OnDeath()
    {
        if (animator != null)
        {
            animator.SetTrigger(Death);
        }
    }

    private void Update()
    {
        if (art != null)
        {
            art.transform.rotation = quaternion.identity;
        }
    }

    private void OnDestroy()
    {
        if (manager != null && manager.HealthComponent != null)
        {
            manager.HealthComponent.OnDamage -= OnTakeDamage;
            manager.HealthComponent.OnDead -= OnDeath;
        }
    }
}