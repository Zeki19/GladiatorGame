using Player;
using UnityEngine;

public class PSWalk<T> : PSBase<T>
{
    private PlayerModel model;
    private PlayerView view;
    private T Trancition;

    private PlayerManager _manager;

    public PSWalk(T trancition, PlayerManager manager)
    {
        Trancition = trancition;
        _manager = manager;
    }
    public override void Enter()
    {
        model=_move as PlayerModel;
        view =_look as PlayerView;
        base.Enter();
        view.PlayStateAnimation(StateEnum.Walk);
        _manager.PlaySound("Walk", "Player");
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
        _manager.PlaySound("Null","Player");
        view.StopStateAnimation(StateEnum.Walk);
    }
}
