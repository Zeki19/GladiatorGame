using UnityEngine;

public class PSWalk<T> : PSBase<T>
{
    T _inputToWalk;
    public PSWalk(T inputToIdle)
    {
        _inputToWalk = inputToIdle;
    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Execute()
    {
        var dir = new Vector2(InputManager.GetMove().x, InputManager.GetMove().y);
        _move.Move(dir);
        if (dir == Vector2.zero)
        {
            StateMachine.Transition(_inputToWalk);
        }
    }
}
