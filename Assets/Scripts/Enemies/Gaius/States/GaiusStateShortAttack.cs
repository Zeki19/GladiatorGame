using Enemies.Hounds.States;
using System.Collections;
using System.Collections.Generic;
using Enemies.Gaius;
using Entities;
using Unity.Mathematics;
using UnityEngine;

public class GaiusStateShortAttack<T> : State_Steering<T>
{
    private SpriteRenderer _spriteRenderer;
    private GaiusController _controller;
    private GaiusModel _model;
    private GaiusView _view;
    private GaiusStatsSO _stats;
    Dictionary<AttackType, float> _attackOptions;

    #region aniamtion

    private AttackType _currentAttack;
    private List<AnimationCurve> _curves;
    private float _animationTime;
    private float _animationClock;

    #endregion
    
    private float _delayTime;
    private float _attackDuration;
    private GameObject _weapon;
    private EnemyManager _manager;
    public GaiusStateShortAttack(ISteering steering, StObstacleAvoidance stObstacleAvoidance, Transform self, SpriteRenderer spriteRenderer, GaiusController GaiusController,GameObject weapon,List<AnimationCurve> curves,GaiusView view) : base(steering, stObstacleAvoidance, self)
    {
        _spriteRenderer = spriteRenderer;
        _controller = GaiusController;
        _stats = GaiusController.stats;
        _attackOptions = new Dictionary<AttackType, float>
        {
            {AttackType.Swipe, 50},
            {AttackType.Lunge, 50}
        }; //HARDCODED.
        _weapon = weapon;
        _curves = curves;
        _view = view;
    }

    public override void Enter()
    {
        base.Enter();
        _spriteRenderer.color = Color.yellow;
        Vector2 dir = _steering.GetDir();
        _move.Move(Vector2.zero);
        var a = _look as GaiusView;
        a.LookDirInsta(dir);
        _model = _attack as GaiusModel;
        _controller.FinishedAttacking = false;
        _currentAttack = MyRandom.Roulette(_attackOptions);
        _weapon.SetActive(true);
        switch(_currentAttack)
        {
            case AttackType.Lunge:
                if (_curves[0].length > 0)
                    _animationTime = _curves[0].keys[_curves[0].length - 1].time;
                _animationClock = 0;
                _controller.StartCoroutine(LungeAttack());
                _view.PlayStateAnimation(StateEnum.ShortAttack);
                break;
            case AttackType.Swipe:
                if (_curves[1].length > 0)
                    _animationTime = _curves[1].keys[_curves[1].length - 1].time;
                _animationClock = 0;
                _weapon.transform.localPosition =
                    _weapon.transform.localPosition.normalized *1.3f;
                _controller.StartCoroutine(SwipeAttack());
                _view.PlayStateAnimation(StateEnum.MidAttack);
                break;
        }
    }

    public override void Execute()
    {
        switch(_currentAttack)
        {
            case AttackType.Lunge:
                _animationClock += Time.deltaTime;
                if (_animationClock < _animationTime)
                {
                    _weapon.transform.localPosition =
                        _weapon.transform.localPosition.normalized *_curves[0].Evaluate(_animationClock);
                }
                break;
            case AttackType.Swipe:
                _animationClock += Time.deltaTime;
                if (_animationClock < _animationTime)
                {
                    _weapon.transform.localRotation = Quaternion.Euler(0, 0, -90+_curves[1].Evaluate(_animationClock));
                    //_weapon.transform.position = _attackPosition;
                }
                break;
        }
    }

    public override void Exit()
    {
        _weapon.transform.localPosition=new Vector3(0,0.5f,0);
        _weapon.transform.localRotation=quaternion.identity;
        _weapon.SetActive(false);
    }

    private IEnumerator LungeAttack()
    {
        _controller.isAttacking = true;
        yield return new WaitForSeconds(_stats.shortDelay);
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
        _controller.isBackStepFinished = false;
        _controller.FinishedAttacking = true;
    }

    private IEnumerator SwipeAttack()
    {
        _controller.isAttacking = true;
        yield return new WaitForSeconds(_stats.mediumDelay);
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
        _controller.isBackStepFinished = false;
        _controller.FinishedAttacking = true;
    }

}
