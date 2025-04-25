using UnityEngine;

public class StatueState_Idle<T> : States_Base<T>
{
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Statue enters idle state");
    }
}
