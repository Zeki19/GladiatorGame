using System.Collections.Generic;
using UnityEngine;

public class FlockingBaseBehaviour : MonoBehaviour, IFlocking
{
    [SerializeField] public float multiplier = 1;
    bool _isActive;
    public Vector2 GetDir(List<IBoid> boids, IBoid self)
    {
        return GetRealDir(boids, self);
    }

    protected virtual Vector2 GetRealDir(List<IBoid> boids, IBoid self)
    {
        return Vector3.zero;
    }
    public bool IsActive { get { return _isActive; } set => _isActive = value; }

}
