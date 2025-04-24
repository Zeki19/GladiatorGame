using UnityEngine;

public class StatueState_Chase<T> : States_Base<T>
{
    private ISteering _steering;

    public StatueState_Chase(ISteering steering)
    {
        _steering = steering;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Statue enters Chase state");
    }

    public override void Execute()
    {
        base.Execute();
        var dir = _steering.GetDir();
        _move.Move(dir.normalized);
        _look.LookDir(dir.normalized);
    }
}
