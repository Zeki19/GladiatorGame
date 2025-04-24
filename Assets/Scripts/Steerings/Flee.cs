using UnityEngine;

public class Flee

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
        //b-->a
        //a-b
        //a: self
        //b: target

        return (_self.position - _target.position).normalized;

    }

}