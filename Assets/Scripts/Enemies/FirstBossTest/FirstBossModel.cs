using UnityEngine;
using Entities;

public class FirstBossModel : EnemyModel
{
    public bool isRested;
    public bool isTired;
    public bool isAttackOnCd;
    public bool isSearchFinish;
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
}
