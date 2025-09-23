using System.Linq;
using Entities;
using UnityEngine;
using Weapons;

public class MovingProjectile : BaseProjectile
{
    private void Update()
    {
        Move();
        if (Vector3.Distance(transform.position, StartingPosition) > MaxRange)
        {
            ServiceLocator.Instance.GetService<ProjectileManager>().ReturnProjectile(this.name, this);
            OnReturnedToPool?.Invoke(this); 
        }
    }

    protected void Move()
    {
        transform.Translate(Vector2.up * (Speed * Time.deltaTime));
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsInLayerMask(other.gameObject, collisionLayer) || Hit.Any(hits => hits == other.gameObject))
            return;

        var entityManager = other.GetComponent<EntityManager>();
        if (entityManager == null)
        {
            ServiceLocator.Instance.GetService<ProjectileManager>().ReturnProjectile(this.name, this);
            OnReturnedToPool?.Invoke(this);
            return;
        };

        entityManager.HealthComponent.TakeDamage(Damage);
        OnHit?.Invoke();
        ServiceLocator.Instance.GetService<ProjectileManager>().ReturnProjectile(this.name, this);
        OnReturnedToPool?.Invoke(this);
        Hit.Add(other.gameObject);
    }
}