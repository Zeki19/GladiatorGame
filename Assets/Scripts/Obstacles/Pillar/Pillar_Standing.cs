using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pillar_Standing : MonoBehaviour, IPillar
{
    [SerializeField] protected SpriteRenderer sprite;
    [SerializeField]private Utilities.BlinkValues blinkDamage;
    private SpriteEffects _blink;
    private Color _originalColor;

    private void Start()
    {
        _originalColor = sprite.color;
    }
    public void AddIgnorePillar(List<Collider2D> colliders)
    {
        _ignoreColliders = colliders;
    }

    public event Action OnDamaged;
    private List<Collider2D> _ignoreColliders = new List<Collider2D>();

    public void SpawnPillar(PillarContext context)
    {
        transform.position = context.Origin.position;
        ServiceLocator.Instance.GetService<NavMeshService>().RebuildNavMesh();

        OnDamaged += Blink;
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
    
    private void Blink()
    {
        _blink = new SpriteEffects(this);
        _blink.Blink(sprite, blinkDamage.amount, blinkDamage.frequency, blinkDamage.blinkActive, _originalColor);
    }
}
