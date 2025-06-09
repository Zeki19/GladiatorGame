using UnityEngine;

public class StPursuit : ISteering
{
    private Transform _self;
    private Rigidbody2D _target;
    private float _timePrediction;


    public StPursuit(Transform self, Rigidbody2D target, float timePrediction)
    {
        _self = self;
        _target = target;
        _timePrediction = timePrediction;
    }
    public StPursuit(Transform self, float timePrediction)
    {
        _self = self;
        _timePrediction = timePrediction;
    }

    public Vector2 GetDir()
    {
        var distanceToTarget = Vector2.Distance(_self.position, _target.position);
        
        Vector2 point = _target.position + _target.linearVelocity * _timePrediction;
        Vector2 dirToPoint = (point - (Vector2)_self.position).normalized;
        Vector2 dirToTarget = (_target.position - (Vector2)_self.position).normalized;
        
        return Vector2.Dot(dirToPoint, dirToTarget) < 0 ? dirToTarget : dirToPoint;
    }
    public Rigidbody2D Target
    {
        set => _target = value;
    }
}
