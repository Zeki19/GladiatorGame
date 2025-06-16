using UnityEngine;

public class GaiusStateChase<T> : State_Steering<T>
{
    private SpriteRenderer _spriteRenderer;
    public GaiusStateChase(ISteering steering, StObstacleAvoidance stObstacleAvoidance, Transform self, SpriteRenderer spriteRenderer) : base(steering, stObstacleAvoidance, self)
    {
        _spriteRenderer = spriteRenderer;
    }

    public override void Enter()
    {
        base.Enter();
        _spriteRenderer.color = Color.white;
        _move.Move(Vector2.zero);
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
