using System;
using System.Collections.Generic;
using Enemies.Hounds.States;
using Entities.StateMachine;
using UnityEngine;
using UnityEngine.UI;

public class StatueState_Attack<T> : StatesBase<T>
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
        _animate.PlayStateAnimation(StateEnum.Attack);

        //if (_target != null && Vector2.Distance(_target.position, _move.Position) <= _attack.AttackRange)
        //{
        //    var health = _target.GetComponent<HealthSystem.HealthSystem>();
        //    if (health != null)
        //    {
        //        health.TakeDamage(_damage);
        //    }
        //}
    }

    public override void Execute()
    {
    }
}
