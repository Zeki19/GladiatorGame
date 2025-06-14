using UnityEngine;

public class GaiusStateShortAttack<T> : States_Base<T>
{
    private FirstBossModel _model;
    private SpriteRenderer _spriteRenderer;
    public GaiusStateShortAttack(SpriteRenderer spriteRenderer)
    {
        _spriteRenderer = spriteRenderer;
    }

    public override void Enter()
    {
        base.Enter();
        _spriteRenderer.color = Color.yellow;
        _move.Move(Vector2.zero);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
