using System.Collections.Generic;
using UnityEngine;
using Entities;

public class FirstBossModel : EnemyModel, IBoid
{
    public List<Vector3Int> Points=new List<Vector3Int>();
    public bool isRested;
    public bool isTired;
    public bool isAttackOnCd;
    public bool isSearchFinish;

    Vector3 IBoid.Position => transform.position;
    public Vector3 Forward => transform.forward;

    public override void ModifySpeed(float speed)
    {
        _speedModifier += speed;
    }

    public override void Move(Vector2 dir)
    {
        dir.Normalize(); //Just in case someone fucks up.
        manager.Rb.linearVelocity = dir * (moveSpeed * _speedModifier);
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
    private void OnDrawGizmos()
    {
        foreach (var VARIABLE in Points)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(VARIABLE, .25f);
        }
    }
}
