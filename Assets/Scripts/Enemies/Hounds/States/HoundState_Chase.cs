using UnityEngine;

public class HoundState_Chase<T> : States_Base<T>
{
    private ISteering _steering;
    private HoundView _view;

    public HoundState_Chase(ISteering steering, HoundView view)
    {
        _steering = steering;
        _view = view;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Hound enters Chase state");
        
        _view.SetChasing(true);
        
    }

    public override void Execute()
    {
        base.Execute();
        var dir = _steering.GetDir();
        _move.Move(dir.normalized);
        _look.LookDir(dir.normalized);
    }

    public override void Exit()
    {
        base.Exit();
        
            _view.SetChasing(false);
    }
}
