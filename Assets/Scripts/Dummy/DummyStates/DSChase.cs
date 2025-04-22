using UnityEngine;

public class DSChase<T> : States_Base<T>
{
    private Transform _target;
    private float _speed;
    
    public DSChase(Transform target, float speed)
    {
        _target = target;
        _speed = speed;
    }
    
    public override void Execute()
    {
        base.Execute(); 
        var dir = (Vector2)_target.transform.position - _move.Position;
       _move.Move(dir);
    }
}
