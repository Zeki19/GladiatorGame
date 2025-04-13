using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState<T>
{
    void Initialize(params object[] p);
    void Enter();
    void Execute(Vector2 direction);
    void FixedExecute();
    void Exit();
    void OnMove(Vector2 direction);
    void OnAttack();
    void OnDash();
    IState<T> GetTransition(T input);
    void AddTransition(T input, IState<T> state);
    void RemoveTransition(T input);
    void RemoveTransition(IState<T> state);
    public FSM<T> StateMachine { get; set; }
}
