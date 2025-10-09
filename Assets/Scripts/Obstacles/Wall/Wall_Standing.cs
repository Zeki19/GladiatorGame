using System;
using Enemies;
using Entities;
using Enemies.Minotaur;
using UnityEngine;


public class Wall_Standing : MonoBehaviour, IPillar
{
    public event Action OnDamaged;

    [SerializeField] private Collider2D hitCollider;

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
