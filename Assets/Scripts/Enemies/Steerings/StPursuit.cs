using UnityEngine;

public class StPursuit : ISteering
{
    private Transform _self;
    private Rigidbody2D _target;


    public StPursuit(Transform self, Rigidbody2D target)
    {
        _self = self;
        _target = target;
    }
    public StPursuit(Transform self)
    {
        _self = self;
    }

    public Vector2 GetDir()
    {
        var distanceToTarget = Vector2.Distance(_self.position, _target.position);
        
        Vector2 point = _target.position + _target.linearVelocity;
        Vector2 dirToPoint = (point - (Vector2)_self.position).normalized;
        Vector2 dirToTarget = (_target.position - (Vector2)_self.position).normalized;
        
        return Vector2.Dot(dirToPoint, dirToTarget) < 0 ? dirToTarget : dirToPoint;
    }
    public Rigidbody2D Target
    {
        set => _target = value;
    }
}
