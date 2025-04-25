using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class HoundState_Runaway<T> : States_Base<T>
{
    private ISteering _steering;
    private HoundView _view;
    public HoundState_Runaway(ISteering steering )
    {
        _steering = steering;
    }
    
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Hound enters Runaway state");
        
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
