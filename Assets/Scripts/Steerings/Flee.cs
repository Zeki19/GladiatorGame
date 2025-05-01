using UnityEngine;

public class Flee : ISteering
{
    private readonly Vector2 _target;
    private readonly Vector2 _self;
 
    public Flee(Vector2 target, Vector2 self)
    {
        _target = target;
        _self = self;
    }

    public Vector2 GetDir()
    {
        return (_self - _target).normalized;
    }
}
