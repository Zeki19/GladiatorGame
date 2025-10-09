using System;
using Entities;
using Enemies.Minotaur;
using UnityEngine;


public class Wall_Standing : MonoBehaviour, IPillar
{
    public event Action OnDamaged;

    [SerializeField] private Collider2D hitCollider;

    // Damage filter
    private bool _onlyMinotaur = true;

    public void ConfigureDamageFilter(bool onlyMinotaur)
    {
        _onlyMinotaur = onlyMinotaur;
    }

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

    private bool IsValidMinotaurHit(Component other)
    {
        // Check Minotaur controller in the hierarchy
        var mino = other.GetComponentInParent<MinotaurController>();
        return mino != null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnDamaged?.Invoke();
        
        return;
        if (IsValidMinotaurHit(collision.collider))
        {
            OnDamaged?.Invoke();
        }
    }
}
