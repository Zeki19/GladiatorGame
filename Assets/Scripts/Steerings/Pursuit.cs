using UnityEngine;

public class Pursuit : ISteering
{
    private readonly Transform _self;
    private readonly Rigidbody _target;
    public float TimePrediction { get; set; }

    public Pursuit(Transform self, Rigidbody target, float timePrediction)
    {
        _self = self;
        _target = target;
        TimePrediction = timePrediction;
    }    
    
    public Pursuit(Transform self, Rigidbody target)
    {
        _self = self;
        _target = target;
    }
    
    public Vector2 GetDir()
    {
        Vector2 point = _target.position + _target.linearVelocity; // Capaz falta sumarle un _target.transform.right?
        Vector2 dirToPoint = (point - (Vector2)_self.position).normalized;
        Vector2 dirToTarget = (_target.position - _self.position).normalized;
        
        return Vector2.Dot(dirToPoint, dirToTarget) < 0 ? dirToTarget : dirToPoint;
    }
}
