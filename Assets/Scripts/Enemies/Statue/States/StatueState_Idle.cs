using System;
using UnityEngine;

public class StatueState_Idle<T> : States_Base<T>
{
    ISteering _steering;
    public StatueState_Idle(ISteering lookAwayDir)
    {
        _steering = lookAwayDir;
    }
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Statue enters idle state");
    }
    public override void Execute()
    {
        base.Execute();
        _look.LookDir(-_steering.GetDir());
        Debug.Log(_steering.GetDir());

    }
    internal void ChangeSteering(ISteering newSteering)
    {
        _steering = newSteering;
    }
}
