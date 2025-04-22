using UnityEngine;

public class Seek : ISteering
{
    private readonly Transform _target;
    private readonly Transform _self;
 
    public Seek(Transform target, Transform self)
    {
        _target = target;
        _self = self;
    }

    public Vector2 GetDir()
    {
        return (_target.position - _self.position).normalized;
    }
}
