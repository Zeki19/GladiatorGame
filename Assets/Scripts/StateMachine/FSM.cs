using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM<T>
{
    IState<T> _currentState;
    public FSM() { }
    public FSM(IState<T> curr)
    {
        SetInit(curr);
    }
    public enum EnemyInputs
    {
        SeePlayer,
        LosePlayer,
        ReachLastPos
    }
    public void SetInit(IState<T> curr)
    {
        curr.StateMachine = this;
        _currentState = curr;
        _currentState.Enter();
    }
    public void OnExecute(Vector2 direction) => _currentState?.Execute(direction);
    public void OnFixedExecute() => _currentState?.FixedExecute();
    public void HandleMove(Vector2 dir) => _currentState?.OnMove(dir);
    public void HandleAttack() => _currentState?.OnAttack();
    public void HandleDash() => _currentState?.OnDash();
    public void Transition(T input)
    {
        IState<T> newState = _currentState.GetTransition(input);
        if (newState == null) return;
        newState.StateMachine = this;
        _currentState.Exit();
        _currentState = newState;
        _currentState.Enter();
    }
}
