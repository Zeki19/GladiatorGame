using UnityEngine;

public class HoundState_Attack<T> : States_Base<T>
{
    private Transform _target;
    
    public HoundState_Attack(Transform target)
    {
        _target = target;
    }
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Hound enters SOAttack state");
    }
}
