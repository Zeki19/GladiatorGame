using System;
using UnityEngine;

public class Wall_Broken : MonoBehaviour, IPillar
{
    public event Action OnDamaged;

    public void SpawnPillar(PillarContext context)
    {
        transform.position = context.Origin.position;
        ServiceLocator.Instance.GetService<NavMeshService>().RebuildNavMesh();
    }

    public void DestroyPillar(PillarContext context)
    {
        OnDamaged = null;
        Destroy(gameObject);
    }
}
