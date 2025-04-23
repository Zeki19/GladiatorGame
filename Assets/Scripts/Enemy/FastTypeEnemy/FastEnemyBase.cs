using UnityEngine;

public class FastEnemyBase<T> : State<T>
{
    protected IMove _move;
    protected ILook _look;
    protected IAttack _attack;
    protected IGoback _back;
    public override void Initialize(params object[] p)
    {
        base.Initialize(p);
        _move = p[0] as IMove;
        _look = p[1] as ILook;
        _attack = p[2] as IAttack;
        _back = p[3] as IGoback;
    }
}

