using UnityEngine;

public class HoundState_Idle<T> : States_Base<T>
{
    public override void Enter()
    {
        base.Enter();
        _move.Move(Vector2.zero);
        _look.PlayStateAnimation(StateEnum.Idle);
    }
}
