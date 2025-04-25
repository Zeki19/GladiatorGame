using UnityEngine;

public class HoundState_Idle<T> : States_Base<T>
{
    private HoundView _view;
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Hound enters idle state");
        
        if (_view is HoundView houndView)
        {
            houndView.PlayIdle();
        }
    }
}
