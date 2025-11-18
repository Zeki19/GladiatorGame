using Player;
using Player.PlayerStates;
using UnityEngine;

public class PSWalk<T> : PSBase<T>
{
    private PlayerModel model;
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
        base.Enter();
        _animate.PlayStateAnimation(StateEnum.Walk);
        _sound.PlaySound("Walk", "Player");
        _manager.PlaySound("Walk");
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
        _sound.PlaySound("Null","Player");
        _manager.StopSounds("Player");
        _animate.StopStateAnimation(StateEnum.Walk);
    }
}
