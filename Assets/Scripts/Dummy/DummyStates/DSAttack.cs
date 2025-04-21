using UnityEngine;

public class DSAttack<T> : DSBase<T>
{
    public override void Enter()
    {
        base.Enter();
        _attack.Attack();
        //Debug.Log("StateAttack");
    }
}
