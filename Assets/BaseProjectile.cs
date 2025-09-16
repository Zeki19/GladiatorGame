using System;
using System.Collections.Generic;
using System.Linq;
using Entities;
using UnityEngine;
using Weapons;

public class BaseProjectile : MonoBehaviour
{
    private readonly List<GameObject> _hit = new List<GameObject>();
    [SerializeField] private LayerMask collisionLayer;
    private float _damage;
    private float _speed;
    private float _maxRange;
    private Vector3 _startingPosition;
    public event Action OnHit;
    public event Action<BaseProjectile> OnReturnedToPool;

    private void Update()
    {
        Move();
        if (Vector3.Distance(transform.position, _startingPosition) > _maxRange)
        {
            ServiceLocator.Instance.GetService<ProjectileManager>().ReturnProjectile(this.name, this);
            OnReturnedToPool?.Invoke(this); 
        }
    }

    protected void Move()
    {
        transform.Translate(Vector2.up * (_speed * Time.deltaTime));
    }
    
    public void Configure(float damage, LayerMask layer, float speed,float maxRange)
    {
        _speed = speed;
        _damage = damage;
        collisionLayer = layer;
        _hit.Clear();
        _maxRange = maxRange;
        _startingPosition=transform.position;
    }
    
    public void ResetState()
    {
        _hit.Clear();
    }

    private void OnEnable()
    {
        _hit.Clear();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsInLayerMask(other.gameObject, collisionLayer) || _hit.Any(hits => hits == other.gameObject))
            return;

        var entityManager = other.GetComponent<EntityManager>();
        if (entityManager == null)
        {
            ServiceLocator.Instance.GetService<ProjectileManager>().ReturnProjectile(this.name, this);
            OnReturnedToPool?.Invoke(this);
            return;
        };

        entityManager.HealthComponent.TakeDamage(_damage);
        OnHit?.Invoke();
        ServiceLocator.Instance.GetService<ProjectileManager>().ReturnProjectile(this.name, this);
        OnReturnedToPool?.Invoke(this);
        _hit.Add(other.gameObject);
    }

    bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return (layerMask.value & (1 << obj.layer)) != 0;
    }
}