using UnityEngine;

public class Pursuit : ISteering

{
    Transform _self;
    Rigidbody2D _target;
    float _timePrediction;

    public Pursuit (Transform self, Rigidbody2D target, float timePrediction)

    {
        _self = self;
        _target = target;
        _timePrediction = timePrediction;
    }

    public Pursuit(Transform self, Rigidbody2D target)

    {
        _self = self;
        _target = target;
       
    }

    public Vector2 GetDirection()
    {
        Vector2 point = _target.position + _target.linearVelocity * _timePrediction;
        //FIJARSE
        Vector2 dirToPoint = (point - (Vector2)_self.position).normalized;
        Vector2 dirToTarget = (_target.position - (Vector2)_self.position).normalized;

        if (Vector2.Dot(dirToPoint, dirToTarget) < 0)
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