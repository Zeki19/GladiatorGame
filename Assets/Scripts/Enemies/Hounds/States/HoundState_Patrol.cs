using UnityEngine;

public class HoundState_Patrol<T> : States_Base<T>
{
    private ISteering _steering;

    public HoundState_Patrol(ISteering steering)
    {
        _steering = steering;
    }
    public override void Enter()
    {
        base.Enter();
        _look.PlayStateAnimation(StateEnum.Patrol);
    }
    public override void Execute()
    {
        base.Execute();
        var dir = _steering.GetDir();
        _move.Move(dir);
        _look.LookDir(dir);
    }
}
