using UnityEngine;

public class HoundState_Patrol<T> : States_Base<T>
{
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Hound enters patrol state");
    }
}
