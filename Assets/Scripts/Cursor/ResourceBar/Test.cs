using System;
using Entities;
using UnityEngine;

public class Test : MonoBehaviour
{
    public ResourceBarTracker rb;

    public EntityManager em;

    private void Start()
    {
        em.HealthComponent.OnDamage += DamageHealthBar;
    }

    private void DamageHealthBar(float a)
    {
        rb.ChangeResourceByAmount((int)a * -1);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            em.HealthComponent.TakeDamage(10);
        }
    }
}
