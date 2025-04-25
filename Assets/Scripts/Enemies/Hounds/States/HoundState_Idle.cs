using UnityEngine;

public class HoundState_Idle<T> : States_Base<T>
{
    private HoundView _view;
    
    public HoundState_Idle (HoundView view)
    {
        _view = view;
    }
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Hound enters idle state");

        _view.PlayIdle();
    }
}
