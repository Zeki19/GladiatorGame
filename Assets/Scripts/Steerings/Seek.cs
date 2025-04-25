using UnityEngine;

public class Seek : ISteering
//va hacia el objetivo (seek)

//le pasamos la entidad y el objetivo:
{

    Transform _self;
    Transform _target;

    public Seek(Transform self, Transform target)
    {
        _self = self;
        _target = target;

    }

    public Vector2 GetDirection()
    {
        //a-->b
        //b-a
        //a: self
        //b: target

        return (_target.position - _self.position).normalized;

    }


}
