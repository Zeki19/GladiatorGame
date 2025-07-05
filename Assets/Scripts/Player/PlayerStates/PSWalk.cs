using Player;
using UnityEngine;

public class PSWalk<T> : PSBase<T>
{
    private PlayerModel model;
    private PlayerView view;
    private T Trancition;

    public PSWalk(T trancition)
    {
        Trancition = trancition;
    }
    public override void Enter()
    {
        model=_move as PlayerModel;
        view =_look as PlayerView;
        base.Enter();
        view?.SetAnimationBool(StateEnum.Walk,true);
    }

    public override void Execute()
    {
        _move.Move(Vector2.zero);
        _look.LookDir(Vector2.zero);
        if(!model.IsPlayerTryingToMove())
            StateMachine.Transition(Trancition);
    }

    public override void Exit()
    {
        base.Exit();
        
        view?.SetAnimationBool(StateEnum.Walk,false);
    }
}
