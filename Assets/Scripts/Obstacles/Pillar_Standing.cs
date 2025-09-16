using System;
using UnityEngine;

public class Pillar_Standing : MonoBehaviour, IPillar
{
    public event Action OnDamaged;

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
        OnDamaged?.Invoke();
    }
}
