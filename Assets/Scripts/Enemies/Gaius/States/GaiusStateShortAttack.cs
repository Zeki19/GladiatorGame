using Enemies.FirstBossTest;
using System;
using System.Collections;
using UnityEngine;
using Weapons.Attacks;
using static Unity.Cinemachine.CinemachineDeoccluder;

public class GaiusStateShortAttack<T> : State_Steering<T>
{
    private SpriteRenderer _spriteRenderer;
    private GaiusController _controller;
    private GaiusModel _model;
    private GaiusStatsSO _stats;
    private float _delayTime;
    private float _attackDuration;
    public GaiusStateShortAttack(ISteering steering, StObstacleAvoidance stObstacleAvoidance, Transform self, SpriteRenderer spriteRenderer, GaiusController GaiusController) : base(steering, stObstacleAvoidance, self)
    {
        _spriteRenderer = spriteRenderer;
        _controller = GaiusController;
        _stats = GaiusController.stats;
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
            _controller.StartCoroutine(Attack());
        }
    }
    private IEnumerator Attack()
    {
        _controller.isAttacking = true;
        yield return new WaitForSeconds(_delayTime);
        Vector2 origin = _controller.transform.position;
        Vector2 facingDir = _controller.transform.up;

        Collider2D[] hits = Physics2D.OverlapCircleAll(_controller.transform.position, _stats.shortRange, _stats.shortTargetLayer);
        foreach (var hit in hits)
        {
            Vector2 toTarget = (Vector2)(hit.transform.position - _controller.transform.position);
            float angleToTarget = Vector2.Angle(facingDir, toTarget);

            if (angleToTarget <= _stats.shortAngle / 2f)
            {
                Debug.Log(hit.name);
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
