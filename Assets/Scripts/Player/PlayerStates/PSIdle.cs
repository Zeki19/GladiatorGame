using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSIdle<T> : PSBase<T>
{
    T _inputToWalk;
    public PSIdle(T inputToWalk)
    {
        _inputToWalk = inputToWalk;
    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Execute(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            StateMachine.Transition(_inputToWalk);
        }
    }
}
