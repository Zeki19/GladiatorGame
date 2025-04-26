using UnityEngine;

public class ToPoint : ISteering
{
    private readonly Vector2 _destination;
    private readonly Transform _self;
    public ToPoint(Vector2 destination, Transform self)
    {
        _destination = destination;
        _self = self;
    }
    public Vector2 GetDir()
    {
        var distanceToTarget = Vector2.Distance(_destination, _self.position);
        return distanceToTarget < 0.5f ? Vector2.zero : (_destination - (Vector2)_self.position).normalized;
    }
}
