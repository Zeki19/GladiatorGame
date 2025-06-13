using UnityEngine;

public class GaiusStateChase<T> : States_Base<T>
{
    private FirstBossModel _model;
    private SpriteRenderer _spriteRenderer;
    public GaiusStateChase(SpriteRenderer spriteRenderer)
    {
        _spriteRenderer = spriteRenderer;
    }

    public override void Enter()
    {
        base.Enter();
        _spriteRenderer.color = Color.blue;
        _move.Move(Vector2.zero);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
