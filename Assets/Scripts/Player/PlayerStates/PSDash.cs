using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PSDash<T> : PSBase<T>
{
    private readonly MonoBehaviour _coroutineRunner;
    private readonly IHealth _characterHealth;
    private readonly PlayerStats _stats;
    private readonly DashIcon _dashIconUI;

    private float _dashTimer = 1f;
    private bool _canDash = true;
    private readonly T _inputFinish;
    private PlayerView _view;

    private PlayerManager _manager;

    public PSDash(T inputFinish, PlayerManager manager, MonoBehaviour coroutineRunner)
    {
        _inputFinish = inputFinish;
        _coroutineRunner = coroutineRunner;
        _stats = manager.stats;
        _characterHealth = manager.HealthComponent;
        _dashIconUI = manager.dashIconUI;

        _manager = manager;
    }
    public override void Enter()
    {
        base.Enter();

        if (!_canDash) return;
        
        _manager.PlaySound("Dash", "Player");
        
        _canDash = false;
        _characterHealth.isInvulnerable = true;

        _move.Dash(_stats.DashForce);
        _dashTimer = _stats.DashDuration;

        _dashIconUI?.HideIcon();
        _coroutineRunner.StartCoroutine(CooldownCoroutine());
        _coroutineRunner.StartCoroutine(InvulnerabilityCooldown());
        
        _view.PlayStateAnimation(StateEnum.Dash);
        
    }
    private IEnumerator CooldownCoroutine()
    {
        yield return new WaitForSeconds(_stats.DashCooldown);
        _canDash = true;
        _dashIconUI?.ShowIcon();
    }
    private IEnumerator InvulnerabilityCooldown()
    {
        float invul = _stats.DashDuration * Mathf.Clamp(_stats.DashInvincibility, 0f, 100f) / 100f;
        yield return new WaitForSeconds(invul);
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
        _move.Move(Vector2.zero);
        _view.StopStateAnimation(StateEnum.Dash);
    }
}