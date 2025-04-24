using UnityEngine;

public class Seek : ISteering
{
    private readonly Vector2 _target;
    private readonly Vector2 _self;
 
    public Seek(Vector2 target, Vector2 self)
    {
        _target = target;
        _self = self;
    }

    public Vector2 GetDir()
    {
        return (_target - _self).normalized;
    }
}
