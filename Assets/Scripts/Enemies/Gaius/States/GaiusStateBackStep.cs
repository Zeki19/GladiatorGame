using Enemies.Gaius;
using Entities;
using UnityEngine;

public class GaiusStateBackStep<T> : States_Base<T>
{
    private SpriteRenderer _spriteRenderer;
    private GaiusController _gaiusController;
    private GaiusStatsSO _stats;
    private float _BackStepTime;
    private EntityManager _gaiusManager;
    public GaiusStateBackStep(SpriteRenderer spriteRenderer, GaiusController GaiusController,EntityManager manager)
    {
        _spriteRenderer = spriteRenderer;
        _gaiusController = GaiusController;
        _stats = GaiusController.stats;
        _BackStepTime = .5f;
        _gaiusManager = manager;
    }

    public override void Enter()
    {
        _BackStepTime = .5f;
        
        _gaiusManager.Sounds?.Invoke("BackStep", "Enemy");
        //_gaiusManager.Rb.bodyType = RigidbodyType2D.Dynamic;
        _move.Dash(3);
    }

    public override void Execute()
    {
        _BackStepTime -= Time.deltaTime;
        
        if (_BackStepTime <= 0.0f)
        {
            _gaiusController.isBackStepFinished = false;
            _gaiusController.FinishedAttacking = false;
            //_gaiusManager.Rb.bodyType = RigidbodyType2D.Kinematic;
            _gaiusManager.Rb.linearVelocity = Vector2.zero;
        }
    }

    public override void Exit()
    {
        _gaiusController.isBackStepFinished = false;
        _gaiusController.FinishedAttacking = false;
        base.Exit();
    }
}