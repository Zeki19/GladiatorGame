using System.Collections;
using System.Collections.Generic;
using Entities;
using Entities.Interfaces;
using Entities.StateMachine;
using UnityEngine;
using UnityEngine.Analytics;

public class State<T> : IState<T>
{
    FSM<T> _fsm;
    Dictionary<T, IState<T>> _transitions = new Dictionary<T, IState<T>>();

    protected IMove _move;
    protected ILook _look;
    protected IAttack _attack;
    protected ISound _sound;
    protected IAnimate _animate;
    protected IStatus _status;
    protected ICondition _condition;
    protected IStatesData _statesData;
    protected INavigation _agent;
    public virtual void Initialize(params object[] p)
    {
        _move = p[0] as IMove;
        _look = p[1] as ILook;
        _attack = p[2] as IAttack;
        _sound = p[3] as ISound;
        _animate = p[4] as IAnimate;
        _status = p[5] as IStatus;
        _condition = p[6] as ICondition;
        _statesData = p[7] as IStatesData;
        _agent = p[8] as INavigation;
    }
    public virtual void Enter()
    {
    }
    public virtual void Execute()
    {
    }
    public virtual void FixedExecute() 
    {
    }
    public virtual void Exit()
    {
    }
    public IState<T> GetTransition(T input)
    {
        if (!_transitions.ContainsKey(input)) return null;
        return _transitions[input];
    }
    public void AddTransition(T input, IState<T> state)
    {
        _transitions[input] = state;
    }
    public void RemoveTransition(T input)
    {
        if (_transitions.ContainsKey(input))
        {
            _transitions.Remove(input);
        }
    }
    public void RemoveTransition(IState<T> state)
    {
        foreach (var item in _transitions)
        {
            if (item.Value == state)
            {
                _transitions.Remove(item.Key);
                break;
            }
        }
    }

    public FSM<T> StateMachine
    {
        set
        {
            _fsm = value;
        }
        get
        {
            return _fsm;
        }
    }
}