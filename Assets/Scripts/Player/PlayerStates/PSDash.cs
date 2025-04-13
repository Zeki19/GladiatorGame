using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PSDash<T> : PSBase<T>
{
    private MonoBehaviour _coroutineRunner;
    private float _dashForce = 1;
    private float _dashDuration = 1;
    private float _dashCooldown = 1;
    private float _dashTimer = 1;

    private bool _canDash = true;


    T _inputFinish;
    public PSDash(T inputFinish, float dashForce, float dashDuration, float dashCooldown, MonoBehaviour coroutineRunner)
    {
        _inputFinish = inputFinish;
        _dashForce = dashForce;
        _dashDuration = dashDuration;
        _dashCooldown = dashCooldown;
        _coroutineRunner = coroutineRunner;
    }
    public override void Enter()
    {
        base.Enter();
        if (!_canDash) 
        {
            return;
        }
        _canDash = false;
        _move.Dash(_dashForce);
        _dashTimer = _dashDuration;
        _coroutineRunner.StartCoroutine(CooldownCoroutine());
    }
    private IEnumerator CooldownCoroutine()
    {
        yield return new WaitForSeconds(_dashCooldown);
        _canDash = true;
    }
    public override void Execute(Vector2 direction)
    {
        base.Execute(direction);
        
        _dashTimer -= Time.deltaTime;
        if (_dashTimer <= 0)
        {
            _move.Move(Vector2.zero, 0);
            StateMachine.Transition(_inputFinish);
        }
    }
}