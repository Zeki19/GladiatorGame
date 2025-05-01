using System;
using System.Collections.Generic;
using Enemies.Hounds.States;
using UnityEngine;
using UnityEngine.UI;

public class StatueState_Attack<T> : States_Base<T>
{
    private Transform _target;
    private float _damage;
    
    public StatueState_Attack(Transform target, float damage)
    {
        _target = target;
        _damage = damage;
    }
    
    public override void Enter()
    { 
        base.Enter();
        _look.PlayStateAnimation(StateEnum.Attack);

        if (_target != null && Vector2.Distance(_target.position, _move.Position) <= _attack.AttackRange)
        {
            var health = _target.GetComponent<HealthSystem>();
            if (health != null)
            {
                health.TakeDamage(_damage);
            }
        }
    }

    public override void Execute()
    {
    }
}
