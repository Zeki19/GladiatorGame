using System.Collections;
using Enemies;
using Entities;
using UnityEngine;

public class EnemyModel : EntityModel
{
    protected float _speedModifier = 1;
    [SerializeField] float _moveSpeed;
    public override void Dash(float dashForce)
    {
        Dash(gameObject.transform.up, dashForce);
    }

    public override void Dash(Vector2 dir, float dashForce)
    {
        manager.Rb.AddForce(dir.normalized * dashForce, ForceMode2D.Impulse);
    }

    public override void Dash(Vector2 dir, float dashForce, float dashDistance)
    {
        Dash(dir, dashForce);
        var controler = manager.controller as EnemyController;
        controler.StartDashMonitoring(dir.normalized, dashDistance, transform.position);
    }

    public override void ModifySpeed(float speed)
    {
        _speedModifier += speed;
    }

    public override void Move(Vector2 dir)
    {
        dir.Normalize();
        manager.Rb.linearVelocity = dir * (_moveSpeed * _speedModifier);
    }

    
    
}

