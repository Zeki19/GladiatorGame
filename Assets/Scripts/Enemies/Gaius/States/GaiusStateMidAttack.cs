using Enemies.FirstBossTest;
using System;
using System.Collections;
using UnityEngine;
using static Unity.Cinemachine.CinemachineDeoccluder;

public class GaiusStateMidAttack<T> : State_Steering<T>
{
    private SpriteRenderer _spriteRenderer;
    private GaiusController _controller;
    private GaiusModel _model;
    private GaiusStatsSO _stats;
    private float _delayTime;
    private float _attackDuration;
    public GaiusStateMidAttack(ISteering steering, StObstacleAvoidance stObstacleAvoidance, Transform self, SpriteRenderer spriteRenderer, GaiusController GaiusController) : base(steering, stObstacleAvoidance, self)
    {
        _spriteRenderer = spriteRenderer;
        _controller = GaiusController;
        _stats = GaiusController.stats;
    }

    public override void Enter()
    {
        base.Enter();
        _delayTime = _stats.mediumDelay;
        _spriteRenderer.color = Color.red;

        Vector2 dir = _steering.GetDir();
        _move.Move(Vector2.zero);
        _look.LookDir(dir);

        _model = _attack as GaiusModel;
    }

    public override void Execute()
    {
        if (!_controller.isAttacking)
        {
            _controller.StartCoroutine(Attack());
        }
    }
    private IEnumerator Attack()
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
        foreach (var hit in hits)
        {
            Debug.Log(hit.name);
            _controller.didAttackMiss = false;
            _model.AttackTarget(hit.transform, _stats.mediumDamage);            
        }
        _controller.isAttacking = false;
    }
}
