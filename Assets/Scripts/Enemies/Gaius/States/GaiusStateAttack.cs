using UnityEngine;

public class GaiusStateAttack<T>: States_Base<T>
{
    private FirstBossModel _model;
    private SpriteRenderer _spriteRenderer;
    public GaiusStateAttack(SpriteRenderer spriteRenderer)
    {
        _spriteRenderer = spriteRenderer;
    }

    public override void Enter()
    {
        base.Enter();
        _spriteRenderer.color = Color.cyan;
        _move.Move(Vector2.zero);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
