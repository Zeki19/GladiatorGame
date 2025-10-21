using System;
using System.Collections.Generic;
using System.Linq;
using Enemies;
using Entities;
using Enemies.Minotaur;
using UnityEngine;


public class Wall_Standing : MonoBehaviour, IPillar
{
    public event Action OnDamaged;

    [SerializeField] private Collider2D hitCollider;
    private List<Collider2D> _ignoreColliders;

    public void SpawnPillar(PillarContext context)
    {
        transform.position = context.Origin.position;
        if (!hitCollider) hitCollider = GetComponent<Collider2D>();
        if (!hitCollider)
        {
            Debug.LogWarning("[Wall_Standing] Missing collider, using default collider on GameObject.");
        }
    }

    public void DestroyPillar(PillarContext context)
    {
        OnDamaged = null;
        Destroy(gameObject);
    }
    public void AddIgnorePillar(List<Collider2D> colliders)
    {
        _ignoreColliders = colliders;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var hit = other.gameObject.GetComponent<EnemyController>();
        if (!hit) return;

        if (hit.GetStatus(StatusEnum.WallBreaker))
        {
            OnDamaged?.Invoke();   
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var hit = collision.gameObject.GetComponent<EnemyController>();
        if (!hit) return;

        if (hit.GetStatus(StatusEnum.WallBreaker))
        {
            OnDamaged?.Invoke();   
        }
    }
}
