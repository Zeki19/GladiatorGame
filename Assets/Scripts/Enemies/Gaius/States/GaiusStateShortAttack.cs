using System.Collections;
using System.Collections.Generic;
using Enemies.Hounds.States;
using Entities.StateMachine;
using Unity.Mathematics;
using UnityEngine;

namespace Enemies.Gaius.States
{
    public class GaiusStateShortAttack<T> : StatesBase<T>
    {
        private SpriteRenderer _spriteRenderer;
        private GaiusController _controller;
        private GaiusStatsSO _stats;
        Dictionary<AttackType, float> _attackOptions;

        #region aniamtion
        private List<AnimationCurve> _curves;
        private float _animationTime;
        private float _animationClock;

        #endregion

        private ISteering _steering;
        private float _delayTime;
        private float _attackDuration;
        private GameObject _weapon;
        public GaiusStateShortAttack(ISteering steering, GaiusController GaiusController,GameObject weapon,List<AnimationCurve> curves)
        {
            _steering = steering;
            _controller = GaiusController;
            _stats = GaiusController.stats;
            _attackOptions = new Dictionary<AttackType, float>
            {
                {AttackType.Swipe, 50},
                {AttackType.Lunge, 50}
            }; //HARDCODED.
            _weapon = weapon;
            _curves = curves;
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
            switch(_controller.currentAttack)
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
                    if (_curves[1].length > 0)
                        _animationTime = _curves[1].keys[_curves[1].length - 1].time;
                    _animationClock = 0;
                    _weapon.transform.localPosition =
                        _weapon.transform.localPosition.normalized *1.3f;
                    _sound.PlaySound("Swipe", "Enemy");
                    _controller.StartCoroutine(SwipeAttack());
                    _animate.PlayStateAnimation(StateEnum.MidAttack);
                    break;
            }
        }

        public override void Execute()
        {
            switch(_controller.currentAttack)
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
            _controller.didAttackMiss = true;
            yield return new WaitForSeconds(_stats.shortDelay);
            _controller.isAttacking = false;
            _controller.isBackStepFinished = false;
            _controller.FinishedAttacking = true;
        }

        private IEnumerator SwipeAttack()
        {
            _controller.isAttacking = true;
            _controller.didAttackMiss = true;
            yield return new WaitForSeconds(_stats.mediumDelay);
            _controller.isAttacking = false;
            _controller.isBackStepFinished = false;
            _controller.FinishedAttacking = true;
        }

    }
}
