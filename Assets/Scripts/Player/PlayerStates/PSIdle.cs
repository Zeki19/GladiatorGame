using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class PSIdle<T> : PSBase<T>
{
    private PlayerModel model;
    private PlayerView view;
    private T Trancition;

    public PSIdle(T trancition)
    {
        Trancition = trancition;
    }
    public override void Enter()
    {
        model=_move as PlayerModel;
        view =_look as PlayerView;
        base.Enter();
    }

    public override void Execute()
    {
        _look.LookDir(Vector2.zero);
        if(model.IsPlayerTryingToMove())
            StateMachine.Transition(Trancition);
    }
}
