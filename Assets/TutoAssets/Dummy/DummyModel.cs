using System;
using System.Collections.Generic;
using UnityEngine;
using Entities;
using Entities.Interfaces;
using Unity.Behavior;

public class DummyModel : EnemyModel
{
    public override void Move(Vector2 dir)
    {
    }

    public override void Dash(float dashForce)
    {
    }

    public override void Dash(Vector2 dir, float dashForce)
    {
    }

    public override void Dash(Vector2 dir, float dashForce, float dashDistance)
    {
    }

    public override void ModifySpeed(float speed)
    {
    }

    public override void SetLinearVelocity(Vector2 velocity)
    {
        manager.Rb.linearVelocity = Vector2.zero;
    }

    public void TakeDamageFromTarget(Transform attacker, float damage)
    {
        if (attacker == null) return;
        manager.HealthComponent.TakeDamage(damage);
    }
}