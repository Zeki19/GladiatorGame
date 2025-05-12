using UnityEngine;

internal class FirstBossState_Idle<T> : States_Base<StateEnum>
{
    public override void Enter()
    {
        Debug.Log("IDLE");
    }
}