using UnityEngine;

public class StSeek : ISteering
{
    private Vector2 _target;
    private Vector2 _self;

    public StSeek(Vector2 target, Vector2 self)
    {
        _target = target;
        _self = self;
    }
    public StSeek(Vector2 self)
    {
        _self = self;
    }

    public Vector2 GetDir()
    {
        return (_target - _self).normalized;
    }
    public Vector2 Target
    {
        set => _target = value;
    }
}
