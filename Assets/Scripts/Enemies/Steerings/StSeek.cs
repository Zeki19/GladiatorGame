using UnityEngine;

public class StSeek : ISteering
{
    private readonly Vector2 _target;
    private readonly Vector2 _self;
 
    public StSeek(Vector2 target, Vector2 self)
    {
        _target = target;
        _self = self;
    }

    public Vector2 GetDir()
    {
        return (_target - _self).normalized;
    }
}
