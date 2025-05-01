using UnityEngine;

public class DSAttack<T> : States_Base<T>
{
    public override void Enter()
    {
        base.Enter();
        _attack.Attack();
    }
}
