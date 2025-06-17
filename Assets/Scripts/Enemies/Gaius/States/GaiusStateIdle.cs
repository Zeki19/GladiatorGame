using Enemies.FirstBossTest;
using Entities;
using UnityEngine;

public class GaiusStateIdle<T> : States_Base<T>
{
    private SpriteRenderer _spriteRenderer;
    private GaiusController _gaiusController;
    private GaiusStatsSO _stats;
    private float _idleTime;
    public GaiusStateIdle(SpriteRenderer spriteRenderer, GaiusController GaiusController)
    {
        _spriteRenderer = spriteRenderer;
        _gaiusController = GaiusController;
        _stats = GaiusController.stats;
        _idleTime = 1f;
    }

    public override void Enter()
    {
        base.Enter();

        switch (StateMachine.LastStateEnum())
        {
            case StateEnum.ShortAttack:
                _idleTime = _stats.shortPunish;
                break;

            case StateEnum.MidAttack:
                _idleTime = _stats.mediumPunish;
                break;

            case StateEnum.LongAttack:
                _idleTime = _stats.longPunish;
                break;

            default:
                Debug.LogError("The idle case you are trying to acces is not contemplated.");
                break;
        }
        _spriteRenderer.color = Color.blue;
        _move.Move(Vector2.zero);
        _look.LookDir(Vector2.zero);
    }

    public override void Execute()
    {
        base.Execute();

        _idleTime -= Time.deltaTime;

        if (_idleTime <= 0.0f)
        {
            _gaiusController.didAttackMiss = false;
        }
    }
}
public class GaiusStateBackStep<T> : States_Base<T>
{
    private SpriteRenderer _spriteRenderer;
    private GaiusController _gaiusController;
    private GaiusStatsSO _stats;
    private float _BackStepTime;
    public GaiusStateBackStep(SpriteRenderer spriteRenderer, GaiusController GaiusController)
    {
        _spriteRenderer = spriteRenderer;
        _gaiusController = GaiusController;
        _stats = GaiusController.stats;
        _BackStepTime = .5f;
    }

    public override void Enter()
    {
        _gaiusController.isBackStepFinished = false;
        _BackStepTime = .5f;
    }

    public override void Execute()
    {
        _BackStepTime -= Time.deltaTime;

        if (_BackStepTime <= 0.0f)
        {
            _gaiusController.isBackStepFinished = true;
        }
    }
}
