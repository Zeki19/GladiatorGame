using Enemies.FirstBossTest;
using UnityEngine;

public class GaiusStateShortAttack<T> : States_Base<T>
{
    private SpriteRenderer _spriteRenderer;
    private GaiusController _gaiusController;
    private GaiusStatsSO _stats;
    private float _delayTime;
    public GaiusStateShortAttack(SpriteRenderer spriteRenderer, GaiusController GaiusController)
    {
        _spriteRenderer = spriteRenderer;
        _gaiusController = GaiusController;
        _stats = GaiusController.stats;
    }

    public override void Enter()
    {
        base.Enter();
        _delayTime = _stats.shortDelay;
        _spriteRenderer.color = Color.yellow;
        _move.Move(Vector2.zero);
    }

    public override void Execute()
    {
        base.Execute();
        _delayTime -= Time.deltaTime;

        if (_delayTime <= 0.0f)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(_gaiusController.transform.position, _stats.shortAngle, _stats.shortTargetLayer);
            foreach (var hit in hits)
            {
                
            }
        }

    }

    public override void Exit()
    {
        base.Exit();
    }
}
