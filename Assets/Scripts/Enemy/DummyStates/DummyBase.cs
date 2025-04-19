using UnityEngine;

public class DummyBase<T> : State<T>
{
    protected Transform _selfTransform;
    protected Transform _target;
    
    public override void Initialize(params object[] args)
    {
        base.Initialize(args);

        _selfTransform = args[0] as Transform;
        _target = args[5] as Transform;
    }

    public override void Execute(Vector2 direction)
    {
        base.Execute(direction);
    }
}
