using System.Collections.Generic;
using Enemies.Hounds.States;
using Entities;
using Player;
using TMPro;
using UnityEngine;

namespace Enemies.FirstBossTest.States
{
    internal class FirstBossStateAttack<T> : States_Base<T>
    {
        private Transform _target;
            private AttackType _chosenType;
            private float _damage;
            private FirstBossModel _model;
            private Dictionary<AttackType, float> _attackOptions;
            private Dictionary<AttackType, float> _lowHpAttackOptions;
            private ChompEffect chompEffect;
        
            private MonoBehaviour _mono;
            private float duration;
            public bool canAttack;
            
        SpriteRenderer _spriteRenderer;
            public FirstBossStateAttack(Transform target, Dictionary<AttackType, float> attackOptions, Dictionary<AttackType, float> lowHpAttackOptions, MonoBehaviour monoBehaviour, float attackCooldown, SpriteRenderer spriteRenderer, ChompEffect chomp)
            {
                _target = target;
                _attackOptions = attackOptions;
                _lowHpAttackOptions = lowHpAttackOptions;
                chompEffect = chomp;
        
                _mono = monoBehaviour;
                duration = attackCooldown;
                _spriteRenderer = spriteRenderer;
            }
            public override void Enter()
            { 
                base.Enter();
                if (_model==null) _model=_move as FirstBossModel;
                Debug.Log("Attack");

                var hp = _model.Manager.HealthComponent;
                var currentHpPercent = hp.GetCurrentHealthPercentage()/100;

                
                Dictionary<AttackType, float> rouletteSource = new Dictionary<AttackType, float>(
                    currentHpPercent <= 0.5f ? _lowHpAttackOptions : _attackOptions
                ); // Usa el diccionario base según la vida 

                // Peso dinámico para ataque Super
                float superWeight = Mathf.Lerp(0f, 80f, 1f - currentHpPercent);
                if (rouletteSource.ContainsKey(AttackType.Super))
                    rouletteSource[AttackType.Super] = superWeight;
                

                _chosenType = MyRandom.Roulette(rouletteSource);
                
                Debug.Log($"[Roulette] HP: {currentHpPercent * 100}% | Chosen Attack: {_chosenType}");

                //Esto deberia ser solo el DMG
                switch (_chosenType)
                {
                  case  AttackType.Normal:
                      _damage = 5;//10
                      break;
                  case AttackType.Charge:
                      _damage = 8;//20
                      break;
                  case AttackType.Lunge:
                      _damage = 10;//15
                      break;
                  case AttackType.Super:
                      _damage = 15;//25
                      break;
                }
                _look.PlayStateAnimation(StateEnum.Attack);
                chompEffect.PlayEffect();
                
                _model.AttackTarget(_target,_damage);
                _model.isAttackOnCd = true;
                _mono.StartCoroutine(AttackCooldown());
            _spriteRenderer.color = Color.red;
            }
        
            private System.Collections.IEnumerator AttackCooldown()
            {
                yield return new WaitForSeconds(duration);
                _model.isAttackOnCd = false;
            }
    }
}