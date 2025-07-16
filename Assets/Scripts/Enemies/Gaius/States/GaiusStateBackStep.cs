using Enemies.Gaius;
using Entities;
using Entities.StateMachine;
using UnityEngine;

public class GaiusStateBackStep<T> : StatesBase<T>
{
    private SpriteRenderer _spriteRenderer;
    private GaiusController _gaiusController;
    private GaiusStatsSO _stats;
    private float _BackStepTime;
    private EntityManager _gaiusManager;
    public GaiusStateBackStep(GaiusController GaiusController)
    {
        _gaiusController = GaiusController;
        _stats = GaiusController.stats;
        _BackStepTime = .5f;
    }

    public override void Enter()
    {
        _BackStepTime = .5f;
        _sound.PlaySound("BackStep", "Enemy");
        _move.Dash(3);
    }

    public override void Execute()
    {
        _BackStepTime -= Time.deltaTime;
        
        if (_BackStepTime <= 0.0f)
        {
            _gaiusController.isBackStepFinished = false;
            _gaiusController.FinishedAttacking = false;
            _move.SetLinearVelocity(Vector2.zero);
        }
    }

    public override void Exit()
    {
        _gaiusController.isBackStepFinished = false;
        _gaiusController.FinishedAttacking = false;
        base.Exit();
    }
}