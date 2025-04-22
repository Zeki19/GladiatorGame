using UnityEngine;

public class HoundState_Runaway<T> : States_Base<T>
{
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Hound starts to runaway (state)");
    }
}
