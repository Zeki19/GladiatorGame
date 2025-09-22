using System.Collections;
using Entities;
using UnityEngine;
using Weapons;

public class GroundBurst : BaseProjectile
{
    [SerializeField] private float burstTimer;
    [SerializeField] private float RemainAfterBurstTimer;

    public override void SetUp(float damage, LayerMask layer, float speed, float maxRange)
    {
        base.SetUp(damage, layer, speed, maxRange);
        StartCoroutine(Burst());
    }

    private IEnumerator Burst()
    {
        yield return new WaitForSeconds(burstTimer);
        var Hits =Physics2D.OverlapCircleAll(transform.position, MaxRange, collisionLayer);
        foreach (var hit in Hits)
        {
            var entityManager = hit.gameObject.GetComponent<EntityManager>();
            if(entityManager == null) continue;
            entityManager.HealthComponent.TakeDamage(Damage);
            OnHit?.Invoke();
            Hit.Add(hit.gameObject);
        }
        yield return new WaitForSeconds(RemainAfterBurstTimer);
        ServiceLocator.Instance.GetService<ProjectileManager>().ReturnProjectile(this.name, this);
        OnReturnedToPool?.Invoke(this);
    }
}