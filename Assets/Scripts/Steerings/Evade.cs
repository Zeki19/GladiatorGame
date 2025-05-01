using UnityEngine;

public class Evade : ISteering
{
    private readonly Transform _self;
    private readonly Rigidbody2D _target;
    public float TimePrediction { get; set; }

    public Evade(Transform self, Rigidbody2D target, float timePrediction)
    {
        _self = self;
        _target = target;
        TimePrediction = timePrediction;
    }

    public Evade(Transform self, Rigidbody2D target)
    {
        _self = self;
        _target = target;
    }

    public Vector2 GetDir()
    {
        Vector2 point = _target.position + _target.linearVelocity; // Capaz falta sumarle un _target.transform.right?
        Vector2 dirToPoint = ((Vector2)_self.position - point).normalized;
        Vector2 dirToTarget = ((Vector2)_self.position - _target.position).normalized;

        return Vector2.Dot(dirToPoint, dirToTarget) < 0 ? dirToTarget : dirToPoint;
    }
}
