using System.Collections.Generic;
using Enemies.Hounds.States;
using Entities;
using UnityEngine;

namespace Enemies.FirstBossTest.States
{
    internal class FirstBossStateAttack<T> : States_Base<T>
    {
        private Transform _target;
            private AttackType _chosenType;
            private float _damage;
            private EntityModel _model;
            private Dictionary<AttackType, float> _attackOptions;
        
            private MonoBehaviour _mono;
            private float duration;
            public bool canAttack;
            
            public FirstBossStateAttack(Transform target, EntityModel model, Dictionary<AttackType, float> attackOptions, MonoBehaviour monoBehaviour, float attackCooldown)
            {
                _target = target;
                _model = model;
                _attackOptions = attackOptions;
        
                _mono = monoBehaviour;
                duration = attackCooldown;
            }
            public override void Enter()
            { 
                base.Enter();
                Debug.Log("Attack");
        
                _chosenType = MyRandom.Roulette(_attackOptions);
                
                //Esto deberia ser solo el DMG
                switch (_chosenType)
                {
                  case  AttackType.Normal:
                      _damage = 10f;
                      break;
                  case AttackType.Charge:
                      _damage = 20f;
                      break;
                  case AttackType.Lunge:
                      _damage = 15f;
                      break;
                }
                _look.PlayStateAnimation(StateEnum.Attack);
                
                PerformAttack();
                canAttack = false;
                _mono.StartCoroutine(AttackCooldown());
            }
            
            void PerformAttack()
            {
                if (_target == null) return;
                
                var health = _target.GetComponent<HealthSystem.HealthSystem>();
                if (health != null)
                {
                    health.TakeDamage(_damage);
                    //_model.Attack();
                }
            }
        
            private System.Collections.IEnumerator AttackCooldown()
            {
                yield return new WaitForSeconds(duration);
                canAttack = true;
            }
    }
}