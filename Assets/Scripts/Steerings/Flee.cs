using UnityEngine;

public class Flee

{
    Transform _self;
    Transform _target;

    public Flee(Transform self, Transform target)
    {
        _self = self;
        _target = target;

    }

    public Vector3 GetDirection()
    {
        //b-->a
        //a-b
        //a: self
        //b: target

        return ( _self.position- _target.position).normalized;

    }

}