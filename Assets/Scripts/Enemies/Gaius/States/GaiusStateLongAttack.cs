using UnityEngine;
using System.Collections;
using Enemies.Gaius;
using System.Collections.Generic;
using Enemies.Hounds.States;
using Entities;

public class GaiusStateLongAttack<T> : State_Steering<T>
{
    private GaiusModel _model;
    private SpriteRenderer _spriteRenderer;
    private GaiusStatsSO _stats;
    private GaiusController _controller;
    Dictionary<AttackType, float> _attackOptions;
    private AttackType _currentAttack;
    private EntityManager _manager;

    public GaiusStateLongAttack(ISteering steering, StObstacleAvoidance stObstacleAvoidance, Transform self, SpriteRenderer spriteRenderer, GaiusController GaiusController, GameObject weapon, List<AnimationCurve> curves, EntityManager manager ) : base(steering, stObstacleAvoidance, self)
    {
        _spriteRenderer = spriteRenderer;
        _controller = GaiusController;
        _stats = GaiusController.stats;
        _attackOptions = new Dictionary<AttackType, float>
        {
            {AttackType.Charge, 50},
            {AttackType.Normal, 50}
        }; //HARDCODED.
        _manager = manager;
    }

    public override void Enter()
    {
        base.Enter();
        _spriteRenderer.color = Color.black;
        _move.Move(Vector2.zero);
        _look.LookDir(Vector2.zero);
        _model = _attack as GaiusModel;


        _currentAttack = MyRandom.Roulette(_attackOptions);
        Debug.Log(_currentAttack);
        switch (_currentAttack)
        {
            case AttackType.Charge:
                    
                    _controller.StartCoroutine(ChargeAttack());
                break;
            default:
                _controller.StartCoroutine(ChargeCooldown());
                break;
        }
    }

    private IEnumerator ChargeAttack()
    {
        _controller.isAttacking = true;
        _controller.FinishedAttacking = false;
        _controller.canLongAttack = false;
        yield return new WaitForSeconds(_stats.longDelay);
        float timer = _stats.longDuration;

        Vector2 hitboxSize = new Vector2(1f, 1f); // Hitbox Size
        Vector2 offset = Vector2.right * 0.75f;     // Forward from boss

        _controller.didAttackMiss = true;
        while (timer > 0f)
        {
            _manager.Rb.bodyType = RigidbodyType2D.Dynamic;
            _manager.Rb.AddForce(_controller.transform.up * _stats.longSpeed);
            timer -= Time.deltaTime;

            Vector2 hitboxCenter = (Vector2)_controller.transform.position + (Vector2.up * 0.75f);
            Collider2D hit = Physics2D.OverlapBox(hitboxCenter, hitboxSize, 0f, _stats.longTargetLayer);

            if (hit && _controller.didAttackMiss)
            {
                _model.AttackTarget(hit.transform, _stats.mediumDamage);
                _controller.didAttackMiss = false;
            }
            yield return null;
        }
        _manager.Rb.bodyType = RigidbodyType2D.Kinematic;
        _controller.isAttacking = false;
        _controller.isBackStepFinished = false;
        _controller.FinishedAttacking = true;
        _controller.canLongAttack = true;
        Debug.Log("Charge Attack!");
    }
    private IEnumerator ChargeCooldown()
    {
        _controller.canLongAttack = false;
        _controller.FinishedAttacking = true;
        yield return new WaitForSeconds(_stats.longIntervalCheck);
        _controller.canLongAttack = true;
    }
}
