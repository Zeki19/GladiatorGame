using Enemies.Hounds.States;
using System.Collections;
using System.Collections.Generic;
using Enemies.Gaius;
using Entities;
using Unity.Mathematics;
using UnityEngine;
using Unity.VisualScripting;

public class GaiusStateMidAttack<T> : State_Steering<T>
{
    private SpriteRenderer _spriteRenderer;
    private GaiusController _controller;
    private GaiusModel _model;
    private GaiusView _view;
    private GaiusStatsSO _stats;

    #region aniamtion
    private List<AnimationCurve> _curves;
    private float _animationTime;
    private float _animationClock;

    #endregion

    private float _delayTime;
    private float _attackDuration;
    private GameObject _weapon;
    private EnemyManager _manager;
    public GaiusStateMidAttack(ISteering steering, StObstacleAvoidance stObstacleAvoidance, Transform self, SpriteRenderer spriteRenderer, GaiusController GaiusController, GameObject weapon, List<AnimationCurve> curves, GaiusView view) : base(steering, stObstacleAvoidance, self)
    {
        _spriteRenderer = spriteRenderer;
        _controller = GaiusController;
        _stats = GaiusController.stats;
        _weapon = weapon;
        _curves = curves;
        _view = view;
    }

    public override void Enter()
    {
        base.Enter();
        Vector2 dir = _steering.GetDir();
        _move.Move(Vector2.zero);
        _view.LookDirInsta(dir);
        _model = _attack as GaiusModel;
        _controller.FinishedAttacking = false;
        _controller.currentAttack = AttackType.Swipe;
        _weapon.SetActive(true);
        if (_curves[2].length > 0)
            _animationTime = _curves[2].keys[_curves[2].length - 1].time;
        _animationClock = 0;
        _weapon.transform.localPosition = _weapon.transform.localPosition.normalized * 1.3f;
        _controller.StartCoroutine(SwipeAttack());
        _view.PlayStateAnimation(StateEnum.MidAttack);
    }

    public override void Execute()
    {
        _animationClock += Time.deltaTime;
        if (_animationClock < _animationTime)
        {
            _weapon.transform.localRotation = Quaternion.Euler(0, 0, -90 + _curves[2].Evaluate(_animationClock));
        }
    }

    public override void Exit()
    {
        _weapon.transform.localPosition = new Vector3(0, 0.5f, 0);
        _weapon.transform.localRotation = quaternion.identity;
        _weapon.SetActive(false);
    }

    private IEnumerator SwipeAttack()
    {
        _controller.isAttacking = true;
        _controller.didAttackMiss = true;
        yield return new WaitForSeconds(_curves[2].keys[_curves[2].length - 1].time);
        _controller.isAttacking = false;
        _controller.isBackStepFinished = false;
        _controller.FinishedAttacking = true;
    }

}
