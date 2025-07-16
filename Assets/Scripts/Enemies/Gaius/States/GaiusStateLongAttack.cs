using System.Collections;
using System.Collections.Generic;
using Enemies.Hounds.States;
using Entities;
using Entities.StateMachine;
using UnityEngine;

namespace Enemies.Gaius.States
{
    public class GaiusStateLongAttack<T> : StatesBase<T>
    {
        private GaiusStatsSO _stats;
        private GaiusController _controller;
        private GameObject _weapon;
        private ISteering _steering;

        public GaiusStateLongAttack(ISteering steering, GaiusController GaiusController, GameObject weapon ) 
        {
            _controller = GaiusController;
            _stats = GaiusController.stats;
            _weapon = weapon;
            _steering = steering;
        }
        public override void Enter()
        {
            base.Enter();
            _move.Move(Vector2.zero);
            _look.LookDirInsta(_steering.GetDir());
            _controller.currentAttack = AttackType.Charge;
            _controller.StartCoroutine(ChargeAttack());
        }

        public override void Exit()
        {
            base.Exit();
            _move.Move(Vector2.zero);
            _look.LookDir(Vector2.zero);
            _weapon.SetActive(false);
        }

        private IEnumerator ChargeAttack()
        {
            _weapon.SetActive(true);
            _controller.isAttacking = true;
            _controller.FinishedAttacking = false;
            _controller.canLongAttack = false;
            _controller.currentAttack = AttackType.Super;
            _sound.PlaySound("Charge", "Enemy");

            yield return new WaitForSeconds(_stats.longDelay);
            float timer = _stats.longDuration;
            Vector3 direction = _controller.transform.up;

            _controller.didAttackMiss = true;
            while (timer > 0f)
            {
                _move.SetLinearVelocity(direction * _stats.longSpeed);
                timer -= Time.deltaTime;
                yield return null;
            }
            _controller.isAttacking = false;
            _controller.isBackStepFinished = false;
            _controller.FinishedAttacking = true;
            _controller.canLongAttack = true;
        }
    }
}
