using System;
using System.Collections.Generic;
using UnityEngine;

public class Wall_Broken : MonoBehaviour, IPillar
{
    private List<Collider2D> _ignoreColliders;
    public event Action OnDamaged;

    public void SpawnPillar(PillarContext context)
    {
        transform.position = context.Origin.position;
        ServiceLocator.Instance.GetService<NavMeshService>().RebuildNavMesh();
    }
    public void AddIgnorePillar(List<Collider2D> colliders)
    {
        _ignoreColliders = colliders;
    }
    public void DestroyPillar(PillarContext context)
    {
        
        OnDamaged = null;
        Destroy(gameObject);
    }
}
