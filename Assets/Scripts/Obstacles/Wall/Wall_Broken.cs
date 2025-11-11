using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Wall_Broken : MonoBehaviour, IPillar
{
    private List<Collider2D> _ignoreColliders;
    public event Action OnDamaged;

    public void SpawnPillar(PillarContext context)
    {
        transform.position = context.Origin.position;
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360f));
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
