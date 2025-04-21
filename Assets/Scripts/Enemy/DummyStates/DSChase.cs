using UnityEngine;

public class DSChase<T> : DSBase<T>
{
    private Transform _target;
    private float _speed;
    
    public DSChase(Transform target, float speed)
    {
        _target = target;
        _speed = speed;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("StateChase");
    }
    public override void Execute()
    {
        base.Execute(); 
        var dir = (Vector2)_target.transform.position - _move.Position;
       _move.Move(Vector2.right, _speed);
    }
}
