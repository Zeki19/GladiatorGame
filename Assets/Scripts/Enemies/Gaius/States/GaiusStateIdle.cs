using UnityEngine;

public class GaiusStateIdle<T> : States_Base<T>
{
    private FirstBossModel _model;
    private SpriteRenderer _spriteRenderer;
    public GaiusStateIdle(SpriteRenderer spriteRenderer)
    {
        _spriteRenderer = spriteRenderer;
    }

    public override void Enter()
    {
        base.Enter();
        _spriteRenderer.color = Color.blue;
        Debug.Log("GAIUS ENTERED IDLE STATE");
        _move.Move(Vector2.zero);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
