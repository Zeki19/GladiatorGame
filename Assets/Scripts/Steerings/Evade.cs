using UnityEngine;

public class Evade : ISteering

{
    Transform _self;
    Rigidbody2D _target;
    float _timePrediction;

    public Evade (Transform self, Rigidbody target, float timePrediction)

    {
        _self = self;
        _target = target;
        _timePrediction = timePrediction;
    }

    public Evade (Transform self, Rigidbody target)

    {
        _self = self;
        _target = target;

    }

    public Vector3 GetDirection()
    {
        Vector3 point = _target.position + _target.linearVelocity * _timePrediction;
        Vector3 dirToPoint = (_self.position- point).normalized;
        Vector3 dirToTarget = (_self.position- _target.position).normalized;

        if (Vector3.Dot(dirToPoint, dirToTarget) < 0)
        {
            return dirToTarget;
        }
        else
        {
            return dirToPoint;
        }

    public float TimePrediction
    {
        get
        {
            return _timePrediction;
        }

        set
        {
            _timePrediction = value;

        }
    }
}