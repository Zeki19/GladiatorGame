using UnityEngine;

public class HoundState_Patrol<T> : States_Base<T>
{
    private ISteering _steering;
    private HoundView _view;
    public HoundState_Patrol(ISteering steering)
    {
        _steering = steering;
    }
    public override void Enter()
    {
        base.Enter();
        Debug.Log("PatrolState"); 
        
        if (_view is HoundView houndView)
        {
            houndView.SetWalking(true);
        }
    }
    public override void Execute()
    {
        base.Execute();
        var dir = _steering.GetDir();
        _move.Move(dir);
        _look.LookDir(dir);
    }
    public void ChangeSteering(ISteering newSteering)
    {
        _steering = newSteering;
    }

    public override void Exit()
    {
        base.Exit();
        
        if (_view is HoundView houndView)
        {
            houndView.SetWalking(false);
        }
    }
}
