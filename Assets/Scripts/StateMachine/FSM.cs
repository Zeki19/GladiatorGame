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
    public void SetInit(IState<T> curr)
    {
        curr.StateMachine = this;
        _currentState = curr;
        _currentState.Enter();
    }
    public void OnExecute()
    {
        if (_currentState != null)
            _currentState.Execute();
    }
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
