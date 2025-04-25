using UnityEngine;

public class Evade : ISteering

{
    Transform _self;
    Rigidbody2D _target;
    float _timePrediction;

    public Evade (Transform self, Rigidbody2D target, float timePrediction)

    {
        _self = self;
        _target = target;
        _timePrediction = timePrediction;
    }

    public Evade (Transform self, Rigidbody2D target)

    {
        _self = self;
        _target = target;

    }

    public Vector2 GetDirection()
    {
        Vector2 point = _target.position + _target.linearVelocity * _timePrediction;

        //FIJARSE
        Vector2 dirToPoint = (_self.position - (Vector3)point).normalized;
        Vector2 dirToTarget = (_self.position - (Vector3)_target.position).normalized;

        if (Vector3.Dot(dirToPoint, dirToTarget) < 0)
        {
            return dirToTarget;
        }
        else
        {
            return dirToPoint;
        }
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