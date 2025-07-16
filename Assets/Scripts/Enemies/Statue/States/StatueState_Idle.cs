using System;
using Entities.StateMachine;
using UnityEngine;

public class StatueState_Idle<T> : StatesBase<T>
{
    ISteering _steering;
    public StatueState_Idle(ISteering lookAwayDir)
    {
        _steering = lookAwayDir;
    }
    public override void Enter()
    {
        base.Enter();
        _move.Move(Vector2.zero);
        _animate.PlayStateAnimation(StateEnum.Idle);
    }
    public override void Execute()
    {
        base.Execute();
        _look.LookDir(-_steering.GetDir());
    }
    internal void ChangeSteering(ISteering newSteering)
    {
        _steering = newSteering;
    }
}
