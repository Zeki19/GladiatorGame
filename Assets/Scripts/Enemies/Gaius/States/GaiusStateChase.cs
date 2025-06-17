using Entities;
using UnityEngine;

public class GaiusStateChase<T> : State_Steering<T>
{
    private SpriteRenderer _spriteRenderer;
    private EntityManager _gaiusManager;
    public GaiusStateChase(ISteering steering, StObstacleAvoidance stObstacleAvoidance, Transform self, SpriteRenderer spriteRenderer,EntityManager manager) : base(steering, stObstacleAvoidance, self)
    {
        _spriteRenderer = spriteRenderer;
        _gaiusManager = manager;
    }

    public override void Enter()
    {
        base.Enter();
        _spriteRenderer.color = Color.white;
        _move.Move(Vector2.zero);
        _gaiusManager.view.PlayStateAnimation(StateEnum.Chase);
    }
    public override void Execute()
    {
        base.Execute();

        Vector2 dir = _steering.GetDir();

        _move.Move(dir);
        _look.LookDir(dir);
    }
    public override void Exit()
    {
        base.Exit();
    }
}
