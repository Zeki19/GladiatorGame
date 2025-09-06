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
        // El dummy no rota, siempre mira hacia adelante
        // O si quieres que mire hacia el atacante, puedes implementarlo aquí
    }

    public override void LookDirInsta(Vector2 dir)
    {
        // El dummy no rota instantáneamente tampoco
    }

    public override void PlayStateAnimation(StateEnum state)
    {
        switch (state)
        {
            case StateEnum.Idle:
                animator.SetTrigger(Idle);
                break;
            default:
                // El dummy solo tiene animación de idle
                animator.SetTrigger(Idle);
                break;
        }
    }

    public override void StopStateAnimation(StateEnum state)
    {
        // No necesita parar animaciones específicas
    }

    private void Start()
    {
        base.Start();
        // Suscribirse al evento de daño para reproducir animación de hit
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
        // Mantiene el arte sin rotación si es necesario
        if (art != null)
        {
            art.transform.rotation = quaternion.identity;
        }
    }

    private void OnDestroy()
    {
        // Desuscribirse de los eventos
        if (manager != null && manager.HealthComponent != null)
        {
            manager.HealthComponent.OnDamage -= OnTakeDamage;
            manager.HealthComponent.OnDead -= OnDeath;
        }
    }
}