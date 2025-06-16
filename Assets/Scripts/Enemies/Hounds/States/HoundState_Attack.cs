using System;
using System.Collections.Generic;
using Enemies.Hounds;
using Enemies.Hounds.States;
using Entities;
using UnityEngine;
using UnityEngine.UI;

public class HoundState_Attack<T> : States_Base<T>
{
    private Transform _target;
    private AttackType _chosenType;
    private float _damage;
    private EntityModel _model;
    private Dictionary<AttackType, float> _attackOptions;
    private Dictionary<AttackType, float> _lowHpAttackOptions;

    private MonoBehaviour _mono;
    private float duration;
    public bool canAttack;
    
    public HoundState_Attack(Transform target, EntityModel model, Dictionary<AttackType, float> attackOptions,Dictionary<AttackType, float> lowHpAttackOptions, MonoBehaviour monoBehaviour, float attackCooldown)
    {
        _target = target;
        _model = model;
        _attackOptions = attackOptions;
        _lowHpAttackOptions = lowHpAttackOptions;

        _mono = monoBehaviour;
        duration = attackCooldown;
    }
    public override void Enter()
    { 
        base.Enter();
        
        var hp = _model.GetComponent<HealthSystem.HealthSystem>();
        var currentHpPercent = hp.GetCurrentHealth() / hp.GetMaxHealth();


        var rouletteSource = currentHpPercent < 0.5f ? _lowHpAttackOptions : _attackOptions;
        _chosenType = MyRandom.Roulette(rouletteSource);
        
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
          case AttackType.Super:
              _damage = 25f;
              break;
        }
        _look.PlayStateAnimation(StateEnum.Attack);
        
        (_model as HoundModel).AttackTarget(_target, _damage);
        canAttack = false;
        _mono.StartCoroutine(AttackCooldown());
    }
    
    private System.Collections.IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(duration);
        canAttack = true;
    }
}
