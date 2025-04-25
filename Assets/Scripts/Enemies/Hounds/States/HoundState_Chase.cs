using UnityEngine;

public class HoundState_Chase<T> : States_Base<T>
{
    private ISteering _steering;
    private HoundView _view;

    public HoundState_Chase(ISteering steering)
    {
        _steering = steering;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Hound enters Chase state");
        
        if (_view is HoundView houndView)
        {
            houndView.SetRunningAway(true);
        }
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
        
        if (_view is HoundView houndView)
        {
            houndView.SetRunningAway(false);
        }
    }
}
