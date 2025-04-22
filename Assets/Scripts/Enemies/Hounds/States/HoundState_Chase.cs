using UnityEngine;

public class HoundState_Chase<T> : States_Base<T>
{
    private ISteering _steering;

    public HoundState_Chase(ISteering steering)
    {
        _steering = steering;
    }

    public override void Execute()
    {
        base.Execute();
        var dir = _steering.GetDir();
        //_move.Move(dir.normalized);
    }
}
