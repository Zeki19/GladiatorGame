using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class HoundState_Runaway<T> : States_Base<T>
{
    private ISteering _steering;
    private ObstacleAvoidance _avoidWalls;
    private Transform _self;
    public HoundState_Runaway(ISteering steering, ObstacleAvoidance avoidWalls,Transform self)
    {
        _steering = steering;
        _self = self;
        _avoidWalls = avoidWalls;
    }

    public override void Enter()
    {
        _move.ModifySpeed(3f);
        base.Enter();
        _look.PlayStateAnimation(StateEnum.Runaway);
    }

    public override void Execute()
    {
        base.Execute();
        var dir = _avoidWalls.GetDir(_self, _steering.GetDir());
        _move.Move(dir);
        _look.LookDir(dir);
    }

    public override void Exit()
    {
        _move.ModifySpeed(1f);
        base.Exit();
    }
}
