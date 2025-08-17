using Entities;
using UnityEngine;

public class ValeriaModel : EnemyModel
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
