using System;
using System.Collections.Generic;
using Enemies.Hounds.States;
using UnityEngine;
using UnityEngine.UI;

public class HoundState_Attack<T> : States_Base<T>
{
    private Transform _target;
    private AttackType _chosenType;
    private float _damage;
    private HoundModel _model;
    private Dictionary<AttackType, float> _attackOptions;
    private T _inputFinish;
    
    public HoundState_Attack(Transform target, HoundModel model, Dictionary<AttackType, float> attackOptions, T inputFinish)
    {
        _target = target;
        _model = model;
        _attackOptions = attackOptions;
        _inputFinish = inputFinish;
    }
    public override void Enter()
    { 
        base.Enter();

        _chosenType = MyRandom.Roulette(_attackOptions);
        
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
    }

    public override void Execute()
    {
        if (_target != null && Vector2.Distance(_target.position, _model.Position) <= _model.AttackRange)
        {
            var health = _target.GetComponent<HealthSystem>();
            if (health != null)
            {
                health.TakeDamage(_damage);
                StateMachine.Transition(_inputFinish);
            }
        }
    }
}
