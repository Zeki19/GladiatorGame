using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSDash<T> : PSBase<T>
{
    float _timer;
    float _seconds = 1;
    T _inputFinish;
    public PSDash(T inputFinish, float seconds = 1)
    {
        _inputFinish = inputFinish;
        _seconds = seconds;
    }
    public override void Enter()
    {
        base.Enter();
        _timer = _seconds;
        _move.Dash();
    }
    public override void Execute()
    {
        base.Execute();
        _timer -= Time.deltaTime;
        if (_timer < 0)
        {
            _move.Move(Vector2.zero);
            StateMachine.Transition(_inputFinish);
        }
    }
}