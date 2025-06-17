using Enemies.FirstBossTest;
using Enemies.Hounds.States;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons.Attacks;
using static Unity.Cinemachine.CinemachineDeoccluder;

public class GaiusStateShortAttack<T> : State_Steering<T>
{
    private SpriteRenderer _spriteRenderer;
    private GaiusController _controller;
    private GaiusModel _model;
    private GaiusStatsSO _stats;
    Dictionary<AttackType, float> _attackOptions;
    private float _delayTime;
    private float _attackDuration;
    public GaiusStateShortAttack(ISteering steering, StObstacleAvoidance stObstacleAvoidance, Transform self, SpriteRenderer spriteRenderer, GaiusController GaiusController) : base(steering, stObstacleAvoidance, self)
    {
        _spriteRenderer = spriteRenderer;
        _controller = GaiusController;
        _stats = GaiusController.stats;
        _attackOptions = new Dictionary<AttackType, float>
        {
            {AttackType.Swipe, 50},
            {AttackType.Lunge, 50}
        }; //HARDCODED.
    }

    public override void Enter()
    {
        base.Enter();
        _delayTime = _stats.shortDelay;
        _spriteRenderer.color = Color.yellow;
        Vector2 dir = _steering.GetDir();
        _move.Move(Vector2.zero);
        _look.LookDir(dir);
        _model = _attack as GaiusModel;
    }

    public override void Execute()
    {
        if (!_controller.isAttacking)
        {
            switch(MyRandom.Roulette(_attackOptions))
            {
                case AttackType.Lunge:
                    _controller.StartCoroutine(LungeAttack());
                    break;
                case AttackType.Swipe:
                    _controller.StartCoroutine(SwipeAttack());
                    break;
            }
            
        }
    }

    private IEnumerator LungeAttack()
    {
        _controller.isAttacking = true;
        yield return new WaitForSeconds(_delayTime);
        Vector2 origin = _model.transform.position;
        Vector2 direction = _model.transform.up.normalized;

        // Calculates center of the lunge hitbox
        Vector2 boxCenter = origin + direction * (_stats.mediumRange / 2f);
        Vector2 boxSize = new Vector2(_stats.mediumRange, _stats.mediumWidth);

        // Rotates the box
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Collider2D[] hits = Physics2D.OverlapBoxAll(boxCenter, boxSize, angle, _stats.mediumTargetLayer);

        _controller.didAttackMiss = true;
        Debug.Log("LUNGE ATTACK");
        foreach (var hit in hits)
        {
            _controller.didAttackMiss = false;
            _model.AttackTarget(hit.transform, _stats.mediumDamage);
        }
        _controller.isAttacking = false;
    }

    private IEnumerator SwipeAttack()
    {
        _controller.isAttacking = true;
        yield return new WaitForSeconds(_delayTime);
        Vector2 origin = _controller.transform.position;
        Vector2 facingDir = _controller.transform.up;

        Collider2D[] hits = Physics2D.OverlapCircleAll(_controller.transform.position, _stats.mediumRange, _stats.shortTargetLayer);
        Debug.Log("SWIPE ATTACK");

        foreach (var hit in hits)
        {
            Vector2 toTarget = (Vector2)(hit.transform.position - _controller.transform.position);
            float angleToTarget = Vector2.Angle(facingDir, toTarget);

            if (angleToTarget <= _stats.shortAngle / 2f)
            {
                _controller.didAttackMiss = false;
                _model.AttackTarget(hit.transform, _stats.shortDamage);
            }
            else 
            {
                _controller.didAttackMiss = true; 
            }
        }
        _controller.isAttacking = false;
    }
}
