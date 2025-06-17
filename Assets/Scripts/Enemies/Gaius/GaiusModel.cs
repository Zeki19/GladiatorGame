using System.Collections.Generic;
using UnityEngine;
using Entities;

public class GaiusModel : EnemyModel
{
    [SerializeField] float _moveSpeed;
    public Vector3 Forward => transform.forward;
    public override void ModifySpeed(float speed)
    {
        _speedModifier += speed;
    }

    public override void Move(Vector2 dir)
    {
        dir.Normalize(); //Just in case someone fucks up.
        manager.Rb.linearVelocity = dir * (_moveSpeed * _speedModifier);
    }
    
    public void AttackTarget(Transform target, float damage)
    {
        if (target == null) return;

        var manager = target.GetComponent<EntityManager>();
        if (manager != null)
        {
            manager.HealthComponent.TakeDamage(damage);
        }
    }

    public override void Dash(float dashForce)
    {
        manager.Rb.AddForce(-gameObject.transform.up*dashForce,ForceMode2D.Impulse);
        Debug.Log(-gameObject.transform.up*dashForce);
    }

    private void OnDrawGizmos()
    {
    }
}
