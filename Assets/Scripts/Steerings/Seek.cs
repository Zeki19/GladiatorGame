using UnityEngine;

public class Seek : ISteering
//va hacia el objetivo (seek)

//le pasamos la entidad y el objetivo:
{

    Transform _self;
    Transform _target;

    public Seek(Transform _self, Transform _target)
    {
        _self = _self;
        _target = _target;

    }

    public Vector3 GetDirection()
    {
        //a-->b
        //b-a
        //a: self
        //b: target

        return (_target.position - _self.position).normalized;

    }


}
