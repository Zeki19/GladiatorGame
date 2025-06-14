using UnityEngine;

public class GaiusStateLongAttack<T> : States_Base<T>
{
    private FirstBossModel _model;
    private SpriteRenderer _spriteRenderer;
    public GaiusStateLongAttack(SpriteRenderer spriteRenderer)
    {
        _spriteRenderer = spriteRenderer;
    }

    public override void Enter()
    {
        base.Enter();
        _spriteRenderer.color = Color.black;
        _move.Move(Vector2.zero);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
