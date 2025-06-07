using Entities.Interfaces;
using UnityEngine;

public class States_Base<T> : State<T>
{
    // This is the base for all the states. Is the parent of State_FollowPoints and State_Steering.
    protected IMove _move;
    protected ILook _look;
    protected IAttack _attack;
    public override void Initialize(params object[] p)
    {
        base.Initialize(p);
        _move = p[0] as IMove;
        _look = p[1] as ILook;
        _attack = p[2] as IAttack;
    }
}
