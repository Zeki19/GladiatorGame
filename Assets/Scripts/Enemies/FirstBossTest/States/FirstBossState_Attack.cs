using UnityEngine;

internal class FirstBossState_Attack<T> : States_Base<StateEnum>
{
    public override void Enter()
    {
        Debug.Log("ATTACK");
    }
}