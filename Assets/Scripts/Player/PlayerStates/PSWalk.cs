using UnityEngine;

public class PSWalk<T> : PSBase<T>
{
    T _inputToStopWalk;
    public PSWalk(T inputToIdle)
    {
        _inputToStopWalk = inputToIdle;
    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Execute()
    {
    }
    public override void FixedExecute() 
    {
    }

    public override void OnMove(Vector2 direction)
    {
        base.OnMove(direction);
        _move.Move(direction);
        //Debug.Log("I'M INSIDE PSWALK");
        if (direction == Vector2.zero)
        {
            StateMachine.Transition(_inputToStopWalk);
        }
    }

    public override void Exit() 
    {
        _move.Move(Vector2.zero);
    }
}
