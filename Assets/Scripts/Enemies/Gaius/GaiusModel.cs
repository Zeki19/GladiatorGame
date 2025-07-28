using System;
using System.Collections.Generic;
using UnityEngine;
using Entities;
using Entities.Interfaces;
using Unity.Behavior;

public class GaiusModel : EnemyModel
{
    public void AttackTarget(Transform target, float damage)
    {
        if (target == null) return;

        var manager = target.GetComponent<EntityManager>();
        if (manager != null)
        {
            manager.HealthComponent.TakeDamage(damage);
        }
    }
}