using UnityEngine;

public class HoundState_Idle<T> : States_Base<T>
{
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Hound enters idle state");
    }
}
