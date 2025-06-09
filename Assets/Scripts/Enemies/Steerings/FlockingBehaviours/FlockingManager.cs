using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class FlockingManager : MonoBehaviour, ISteering
{
    [Min(1)]
    [SerializeField] public int maxBoids = 10;
    [Min(1)]
    [SerializeField] public int radius = 10;
    [SerializeField] public LayerMask boidMask;
    IFlocking[] _behaviours;
    IBoid _self;
    Collider2D[] _colliders;
    List<IBoid> _boids;
    private void Awake()
    {
        _behaviours = GetComponents<IFlocking>();
        _self = GetComponent<IBoid>();
        _colliders = new Collider2D[maxBoids];
        _boids = new List<IBoid>();
    }

    public Vector2 GetDir()
    {
        _boids.Clear();
        _colliders = Physics2D.OverlapCircleAll(_self.Position, radius, boidMask);
        int count = _colliders.Length;
        Debug.Log(count);
        for (int i = 0; i < count; i++)
        {
            var boid = _colliders[i].GetComponent<IBoid>();
            if (boid == null || boid == _self) continue;
            _boids.Add(boid);
        }
        Vector2 dir = Vector2.zero;
        for (int i = 0; i < _behaviours.Length; i++) 
        {
            var curr = _behaviours[i];
            dir += curr.GetDir(_boids, _self);
        }

        return dir.normalized;
    }

    public IBoid SetSelf { set => _self = value; }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

}
