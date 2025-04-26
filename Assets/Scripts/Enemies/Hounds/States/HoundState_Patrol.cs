using UnityEngine;

public class HoundState_Patrol<T> : States_Base<T>
{
    private ISteering _steering;
    private ObstacleAvoidance _avoidWalls;
    private Transform _self;

    public HoundState_Patrol(ISteering steering,ObstacleAvoidance avoidWalls, Transform self)
    {
        _steering = steering;
        _self = self;
        _avoidWalls = avoidWalls;

    }
public override void Enter()
    {
        base.Enter();
        _look.PlayStateAnimation(StateEnum.Patrol);
    }
    public override void Execute()
    {
        base.Execute();
        var dir = _avoidWalls.GetDir(_self, _steering.GetDir());
        _move.Move(dir);
        _look.LookDir(dir);
    }
}
