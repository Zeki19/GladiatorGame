using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSAttack<T> : PSBase<T>
{
    float _timer;
    float _seconds = 1;
    float _moveSpeed;
    T _inputFinish;
    public PSAttack(T inputFinish, float moveSpeed, float seconds = 1)
    {
        _inputFinish = inputFinish;
        _seconds = seconds;
        _moveSpeed = moveSpeed;
    }
    public override void Enter()
    {
        base.Enter();
        _timer = _seconds;
        _attack.Attack();
    }
    public override void Execute()
    {
        base.Execute();
        //_move.Move(direction, _moveSpeed);
        _timer -= Time.deltaTime;
        if (_timer < 0)
        {
            StateMachine.Transition(_inputFinish);
        }

    }
}
