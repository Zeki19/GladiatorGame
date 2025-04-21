using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PSDash<T> : PSBase<T>
{
    private MonoBehaviour _coroutineRunner;
    private float _dashForce = 1;
    private float _dashDuration = 1;
    private float _dashCooldown = 1;
    private float _dashTimer = 1;
    private float _dashInvincibility = 1;
    private IHealth _characterHealth;

    private bool _canDash = true;

    T _inputFinish;
    public PSDash(T inputFinish, float dashForce, float dashDuration, float dashCooldown, float dashInvincibility, IHealth healthSystem,MonoBehaviour coroutineRunner)
    {
        _inputFinish = inputFinish;
        _dashForce = dashForce;
        _dashDuration = dashDuration;
        _dashCooldown = dashCooldown;
        _coroutineRunner = coroutineRunner;
        _dashInvincibility = Mathf.Clamp(dashInvincibility, 0, 100);
        _characterHealth = healthSystem;
    }
    public override void Enter()
    {
        base.Enter();
        if (!_canDash) 
        {
            return;
        }
        _canDash = false;
        _characterHealth.isInvulnerable = true;
        _move.Dash(_dashForce);
        _dashTimer = _dashDuration;
        _coroutineRunner.StartCoroutine(CooldownCoroutine());
    }
    private IEnumerator CooldownCoroutine()
    {
        yield return new WaitForSeconds(_dashCooldown);
        _canDash = true;
    }

    private IEnumerator InvulnerabilityCooldown()
    {
        yield return new WaitForSeconds((_dashCooldown*_dashInvincibility)/100);
        _characterHealth.isInvulnerable = false;
    }
    public override void Execute()
    {
        base.Execute();
        
        _dashTimer -= Time.deltaTime;
        if (_dashTimer <= 0)
        {
            StateMachine.Transition(_inputFinish);
        }
    }

    public override void Exit()
    {
        base.Exit();
        _move.Move(10); //This is here so that the player doesn't go on infinitively with the dash if no input is being received. <-- MANSO CHOCLO
    }
}