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
        private GaiusModel _model;
        private SpriteRenderer _spriteRenderer;
        private GaiusStatsSO _stats;
        private GaiusController _controller;
        Dictionary<AttackType, float> _attackOptions;
        private EntityManager _manager;
        private GameObject _weapon;
        private ISteering _steering;

        public GaiusStateLongAttack(ISteering steering, StObstacleAvoidance stObstacleAvoidance, Transform self, SpriteRenderer spriteRenderer, GaiusController GaiusController, GameObject weapon, List<AnimationCurve> curves, EntityManager manager ) 
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
            _weapon = weapon;
            _steering = steering;
        }
        public override void Enter()
        {
            base.Enter();
            _move.Move(Vector2.zero);
            _model = _attack as GaiusModel;
            _look.LookDirInsta(_steering.GetDir());
            _controller.currentAttack = AttackType.Charge;
            _controller.StartCoroutine(ChargeAttack());
            //switch (_controller.currentAttack)
            //{
            //    case AttackType.Charge:
            //            
            //        _controller.StartCoroutine(ChargeAttack());
            //        break;
            //    default:
            //        _controller.StartCoroutine(ChargeCooldown());
            //        break;
            //}
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
            _sound.PlaySound("Charge", "Enemy");

            yield return new WaitForSeconds(_stats.longDelay);
            float timer = _stats.longDuration;
            Vector3 direction = _controller.transform.up;
            Vector2 hitboxSize = new Vector2(1.5f, 1.5f);

            _controller.didAttackMiss = true;
            while (timer > 0f)
            {
                _move.SetLinearVelocity(direction * _stats.longSpeed);
                timer -= Time.deltaTime;

                Vector2 hitboxCenter = (Vector2)_controller.transform.position + ((Vector2)direction * 0.75f);
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Collider2D hit = Physics2D.OverlapBox(hitboxCenter, hitboxSize, angle, _stats.longTargetLayer);

                if (hit && _controller.didAttackMiss)
                {
                    _model.AttackTarget(hit.transform, _stats.longDamage);
                    _controller.didAttackMiss = false;
                }
                yield return null;
            }
            _controller.isAttacking = false;
            _controller.isBackStepFinished = false;
            _controller.FinishedAttacking = true;
            _controller.canLongAttack = true;
        }
        private IEnumerator ChargeCooldown()
        {
            _controller.canLongAttack = false;
            _controller.FinishedAttacking = true;
            _controller.didAttackMiss = true;
            yield return new WaitForSeconds(_stats.longIntervalCheck);
            _controller.canLongAttack = true;
        }
    }
}
