using Enemies.Hounds.States;
using System.Collections;
using System.Collections.Generic;
using Enemies.Gaius;
using Entities;
using Entities.StateMachine;
using Unity.Mathematics;
using UnityEngine;
using Unity.VisualScripting;

public class GaiusStateMidAttack<T> : StatesBase<T>
{
    
    private GaiusController _controller;
    Dictionary<AttackType, float> _attackOptions;
    private int _pokeCounter = 0;
    #region aniamtion
    private List<AnimationCurve> _curves;
    private float _animationTime;
    private float _animationClock;

    #endregion
    ISteering _steering;
    private float _delayTime;
    private float _attackDuration;
    private GameObject _weapon;
    public GaiusStateMidAttack(ISteering steering, GaiusController GaiusController, GameObject weapon, List<AnimationCurve> curves)
    {
        _controller = GaiusController;
        _weapon = weapon;
        _curves = curves;
        _attackOptions = new Dictionary<AttackType, float>
        {
            {AttackType.Swipe, 50},
            {AttackType.Lunge, 50}
        }; //HARDCODED.
        _steering = steering;
    }

    public override void Enter()
    {
        base.Enter();
        Vector2 dir = _steering.GetDir();
        _move.Move(Vector2.zero);
        _look.LookDirInsta(dir);
        _controller.FinishedAttacking = false;
        _controller.currentAttack = MyRandom.Roulette(_attackOptions);
        _weapon.SetActive(true);
        switch (_controller.currentAttack)
        {
            case AttackType.Lunge:
                if (_curves[0].length > 0)
                    _animationTime = _curves[0].keys[_curves[0].length - 1].time;
                _animationClock = 0;
                _sound.PlaySound("Lunge", "Enemy");
                _controller.StartCoroutine(LungeAttack());
                _animate.PlayStateAnimation(StateEnum.ShortAttack);
                break;
            case AttackType.Swipe:
                if (_curves[2].length > 0)
                    _animationTime = _curves[2].keys[_curves[2].length - 1].time;
                _animationClock = 0;
                _weapon.transform.localPosition =
                    _weapon.transform.localPosition.normalized * 1.3f;
                _sound.PlaySound("Swipe", "Enemy");
                _controller.StartCoroutine(SwipeAttack());
                _animate.PlayStateAnimation(StateEnum.MidAttack);
                break;
        }
        _weapon.transform.localPosition = _weapon.transform.localPosition.normalized * 1.3f;
        _controller.StartCoroutine(SwipeAttack());
        _animate.PlayStateAnimation(StateEnum.MidAttack);
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
        _sound.PlaySound("MegaSwing", "Enemy");
        yield return new WaitForSeconds(_curves[2].keys[_curves[2].length - 1].time);
        _controller.isAttacking = false;
        _controller.isBackStepFinished = false;
        _controller.FinishedAttacking = true;
    }

}
