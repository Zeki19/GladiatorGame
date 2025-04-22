using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class HoundState_Runaway<T> : States_Base<T>
{
    private Vector2 _destination;
    public HoundState_Runaway(Vector2 point)
    {
        _destination = point;
    }
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Hound starts to runaway (state)");
    }

    public override void Execute()
    {
        base.Execute();
        _move.Move(5f);
    }
}
