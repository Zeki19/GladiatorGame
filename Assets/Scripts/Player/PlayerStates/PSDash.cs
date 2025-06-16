using System.Collections;
using System.Collections.Generic;
using Player;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PSDash<T> : PSBase<T>
{
    private MonoBehaviour _coroutineRunner;

    private float _dashTimer = 1;
    private PlayerStats _stats;
    private IHealth _characterHealth;

    private bool _canDash = true;

    T _inputFinish;

    public PSDash(T inputFinish, PlayerManager manager, MonoBehaviour coroutineRunner)
    {
        _stats = manager.stats;
        _inputFinish = inputFinish;
        _coroutineRunner = coroutineRunner;
        _characterHealth = manager.HealthComponent;
    }

    public override void Enter()
    {
        base.Enter();
        if (!_canDash) return;
        _canDash = false;
        _characterHealth.isInvulnerable = true;
        _move.Dash(_stats.DashForce);
        _dashTimer = _stats.DashDuration;
        _coroutineRunner.StartCoroutine(CooldownCoroutine());
        _coroutineRunner.StartCoroutine(InvulnerabilityCooldown());
    }

    private IEnumerator CooldownCoroutine()
    {
        yield return new WaitForSeconds(_stats.DashCooldown);
        _canDash = true;
    }

    private IEnumerator InvulnerabilityCooldown()
    {
        yield return new WaitForSeconds(_stats.DashCooldown * Mathf.Clamp(_stats.DashInvincibility, 0, 100) / 100);
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
        _move.Move(Vector2
            .zero); //This is here so that the player doesn't go on infinitively with the dash if no input is being received. <-- MANSO CHOCLO
    }
}