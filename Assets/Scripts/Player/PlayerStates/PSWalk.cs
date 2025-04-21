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

    public override void Execute()
    {
        _move.Move(_moveSpeed);
    }
}
