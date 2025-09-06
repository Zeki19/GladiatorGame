using System;
using System.Collections.Generic;
using UnityEngine;
using Entities;
using Entities.Interfaces;
using Unity.Behavior;

public class DummyModel : EnemyModel
{
    // El dummy override los métodos de movimiento para que no se mueva
    public override void Move(Vector2 dir)
    {
        // No hace nada - se queda quieto
    }

    public override void Dash(float dashForce)
    {
        // No hace nada - no puede dashear
    }

    public override void Dash(Vector2 dir, float dashForce)
    {
        // No hace nada - no puede dashear
    }

    public override void Dash(Vector2 dir, float dashForce, float dashDistance)
    {
        // No hace nada - no puede dashear
    }

    public override void ModifySpeed(float speed)
    {
        // No modifica velocidad ya que no se mueve
    }

    public override void SetLinearVelocity(Vector2 velocity)
    {
        // Siempre mantiene velocidad en cero
        manager.Rb.linearVelocity = Vector2.zero;
    }

    // Método para recibir daño (mantiene la funcionalidad de dummy)
    public void TakeDamageFromTarget(Transform attacker, float damage)
    {
        if (attacker == null) return;

        // El dummy solo recibe daño, no contraataca
        manager.HealthComponent.TakeDamage(damage);
    }
}