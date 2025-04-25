using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class HoundState_Runaway<T> : States_Base<T>
{
    private ISteering _steering;
    private HoundView _view;
    
    
    public HoundState_Runaway(ISteering steering, HoundView view )
    {
        _steering = steering;
        _view = view;
    }
    
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Hound enters Runaway state");

        _view.SetRunningAway(true);
        
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

        _view.SetRunningAway(false);
        
    }
}
