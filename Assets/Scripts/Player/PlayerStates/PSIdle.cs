using System.Collections;
using System.Collections.Generic;
using Player;
using Player.PlayerStates;
using UnityEngine;

public class PSIdle<T> : PSBase<T>
{
    private PlayerModel model;
    private T Trancition;

    public PSIdle(T trancition)
    {
        Trancition = trancition;
    }
    public override void Enter()
    {
        model=_move as PlayerModel;
        base.Enter();
    }

    public override void Execute()
    {
        _look.LookDir(Vector2.zero);
        if(model.IsPlayerTryingToMove())
            StateMachine.Transition(Trancition);
    }
}
