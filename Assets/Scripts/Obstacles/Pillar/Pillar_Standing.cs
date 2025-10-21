using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pillar_Standing : MonoBehaviour, IPillar
{
    public void AddIgnorePillar(List<Collider2D> colliders)
    {
        _ignoreColliders = colliders;
    }

    public event Action OnDamaged;
    private List<Collider2D> _ignoreColliders = new List<Collider2D>();

    public void SpawnPillar(PillarContext context)
    {
        transform.position = context.Origin.position;
    }

    public void DestroyPillar(PillarContext context)
    {
        OnDamaged = null;
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_ignoreColliders.Any(collider => collider == other))
        {
            return;
        }
        OnDamaged?.Invoke();
    }
}
