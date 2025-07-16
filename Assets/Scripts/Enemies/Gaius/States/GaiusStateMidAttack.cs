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
    Dictionary<AttackType, float> _attackOptions;
    private int _pokeCounter = 0;

    #region aniamtion
    private List<AnimationCurve> _curves;
    private float _animationTime;
    private float _animationClock;

    #endregion

    private float _delayTime;
    private float _attackDuration;
    private GameObject _weapon;
    private EnemyManager _manager;
    public GaiusStateMidAttack(ISteering steering, StObstacleAvoidance stObstacleAvoidance, Transform self, SpriteRenderer spriteRenderer, GaiusController GaiusController, GameObject weapon, List<AnimationCurve> curves, GaiusView view, EnemyManager manager) : base(steering, stObstacleAvoidance, self)
    {
        _spriteRenderer = spriteRenderer;
        _controller = GaiusController;
        _stats = GaiusController.stats;
        _weapon = weapon;
        _curves = curves;
        _view = view;
        _manager = manager;
        _attackOptions = new Dictionary<AttackType, float>
        {
            {AttackType.Swipe, 50},
            {AttackType.Lunge, 50}
        }; //HARDCODED.
    }

    public override void Enter()
    {
        base.Enter();
        Vector2 dir = _steering.GetDir();
        _move.Move(Vector2.zero);
        _view.LookDirInsta(dir);
        _model = _attack as GaiusModel;
        _controller.FinishedAttacking = false;
        _controller.currentAttack = MyRandom.Roulette(_attackOptions);
        _weapon.SetActive(true);
        switch (_controller.currentAttack)
        {
            case AttackType.Lunge:
                if (_curves[0].length > 0)
                    _animationTime = _curves[0].keys[_curves[0].length - 1].time;
                _animationClock = 0;
                _manager.PlaySound("Lunge", "Enemy");
                _controller.StartCoroutine(LungeAttack());
                _view.PlayStateAnimation(StateEnum.ShortAttack);
                break;
            case AttackType.Swipe:
                if (_curves[2].length > 0)
                    _animationTime = _curves[2].keys[_curves[2].length - 1].time;
                _animationClock = 0;
                _weapon.transform.localPosition =
                    _weapon.transform.localPosition.normalized * 1.3f;
                _manager.PlaySound("Swipe", "Enemy");
                _controller.StartCoroutine(SwipeAttack());
                _view.PlayStateAnimation(StateEnum.MidAttack);
                break;
        }
        _weapon.transform.localPosition = _weapon.transform.localPosition.normalized * 1.3f;
        _controller.StartCoroutine(SwipeAttack());
        _view.PlayStateAnimation(StateEnum.MidAttack);
    }

    public override void Execute()
    {
        _animationClock += Time.deltaTime;
        switch (_controller.currentAttack)
        {
            case AttackType.Lunge:
                _animationClock += Time.deltaTime;
                if (_animationClock < _animationTime && _pokeCounter < 2)
                {
                    _weapon.transform.localPosition =
                        _weapon.transform.localPosition.normalized * _curves[0].Evaluate(_animationClock);
                }
                else if (_pokeCounter < 2)
                {
                    _animationClock = 0;
                    _pokeCounter++;
                }
                break;
            case AttackType.Swipe:
                if (_animationClock < _animationTime)
                {
                    _weapon.transform.localRotation = Quaternion.Euler(0, 0, -90 + _curves[2].Evaluate(_animationClock));
                }
                break;
        }
    }

    public override void Exit()
    {
        _weapon.transform.localPosition = new Vector3(0, 0.5f, 0);
        _weapon.transform.localRotation = quaternion.identity;
        _weapon.SetActive(false);
        _pokeCounter = 0;
    }
    private IEnumerator LungeAttack()
    {
        _controller.isAttacking = true;
        _controller.didAttackMiss = true;
        yield return new WaitForSeconds(_curves[0].keys[_curves[0].length - 1].time * 3);
        _controller.isAttacking = false;
        _controller.isBackStepFinished = false;
        _controller.FinishedAttacking = true;
    }
    private IEnumerator SwipeAttack()
    {
        _controller.isAttacking = true;
        _controller.didAttackMiss = true;
        _manager.PlaySound("MegaSwing", "Enemy");
        yield return new WaitForSeconds(_curves[2].keys[_curves[2].length - 1].time);
        _controller.isAttacking = false;
        _controller.isBackStepFinished = false;
        _controller.FinishedAttacking = true;
    }

}
