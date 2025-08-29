using System;
using Entities;
using UnityEngine;

public class Test : MonoBehaviour
{
    public ResourceBarTracker rb;

    public EntityManager em;

    private void Start()
    {
        em.HealthComponent.OnDamage += Damage;
        em.HealthComponent.OnHeal += Heal;
        em.HealthComponent.OnDead += Dead;
    }

    private void Damage(float a)
    {
        rb.ChangeResourceByAmount((int)a * -1);
    }
    private void Heal(float a)
    {
        rb.ChangeResourceByAmount((int)a);
    }

    private void Dead()
    {
        em.HealthComponent.OnDamage -= Damage;
        em.HealthComponent.OnHeal -= Heal;
        em.HealthComponent.OnDead -= Dead;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            em.HealthComponent.TakeDamage(10);
        }
        if (Input.GetMouseButtonDown(1))
        {
            em.HealthComponent.Heal(10);
        }
    }
}
