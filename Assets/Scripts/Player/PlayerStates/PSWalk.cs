using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PSWalk<T> : PSBase<T>
{
    T _inputToStopWalk;
    float _moveSpeed;
    public PSWalk(T inputToIdle, float moveSpeed)
    {
        _inputToStopWalk = inputToIdle;
        _moveSpeed = moveSpeed;
    }
    public override void Enter()
    {
        base.Enter();
    }

    public override void Execute(Vector2 direction)
    {
        base.OnMove(direction);
        _move.Move(direction, _moveSpeed);
        if (direction == Vector2.zero)
        {
            StateMachine.Transition(_inputToStopWalk);
        }
    }
}
