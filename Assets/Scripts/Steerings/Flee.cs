using UnityEngine;

public class Flee : ISteering
{
    private readonly Transform _target;
    private readonly Transform _self;
 
    public Flee(Transform target, Transform self)
    {
        _target = target;
        _self = self;
    }

    public Vector2 GetDir()
    {
        return (_self.position - _target.position).normalized;
    }
}
