using UnityEngine;

public class StatueState_Runaway<T> : States_Base<T>
{
    private ISteering _steering;
    Vector2 dir;
    public StatueState_Runaway(ISteering steering)
    {
        _steering = steering;
    }
    
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Statue enters Runaway state");
    }

    public override void Execute()
    {
        base.Execute();
        var dir = _steering.GetDir();
        _move.Move(dir.normalized);
        _look.LookDir(dir.normalized);
    }
    public void ChangeSteering(ISteering newSteering)
    {
        _steering = newSteering;
    }

}