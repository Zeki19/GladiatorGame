using UnityEngine;

public class HoundState_Chase<T> : States_Base<T>
{
    private ISteering _steering;
    private ObstacleAvoidance _avoidWalls;
    private Transform _self;
    public HoundState_Chase(ISteering steering, ObstacleAvoidance avoidWalls, Transform self)
    {
        _steering = steering;
        _self = self;
        _avoidWalls = avoidWalls;
    }

    public override void Enter()
    {
        base.Enter();
        _move.ModifySpeed(2f);
        _look.PlayStateAnimation(StateEnum.Chase);
    }
    public override void Execute()
    {
        base.Execute();
        var dir = _avoidWalls.GetDir(_self, _steering.GetDir()); 
        _move.Move(dir.normalized);
        _look.LookDir(dir.normalized);
    }

    public override void Exit()
    {
        _move.ModifySpeed(1f);
        base.Exit();
    }
}
