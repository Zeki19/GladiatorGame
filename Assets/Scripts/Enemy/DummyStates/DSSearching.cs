using UnityEngine;

public class DSSearching<T> : DummyBase<T>
{
    private T _transitionToChase;
    
    public DSSearching(T transitionToChase)
    {
        _transitionToChase = transitionToChase;
    }
    
    public override void Enter()
    {
        base.Enter();
    }
    
    public override void Execute(Vector2 direction)
    {
        base.Execute(direction);

        if (_target != null)
        {
            Vector2 toTarget = _target.position - _selfTransform.position;
            float angle = Vector2.Angle(_selfTransform.right, toTarget.normalized);
            
            if (angle <= 22.5f)
            {
                StateMachine.Transition(_transitionToChase);
            }
        }
    }
}
