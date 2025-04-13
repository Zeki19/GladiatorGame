using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;
public class PlayerController : MonoBehaviour
{
    private FSM<StateEnum> _fsm;
    private void Awake()
    { 
        InitializeFSM();
    }

    private void InitializeFSM()
    {
        _fsm = new FSM<StateEnum>();
        var move = GetComponent<IMove>();
        var look = GetComponent<ILook>();
        var attack = GetComponent<IAttack>();

        var stateList = new List<PSBase<StateEnum>>();

        var idle = new PSIdle<StateEnum>(StateEnum.Walk);
        var walk = new PSWalk<StateEnum>(StateEnum.Idle);
        var spin = new PSSpin<StateEnum>(StateEnum.Idle);

        idle.AddTransition(StateEnum.Walk, walk);
        idle.AddTransition(StateEnum.Spin, spin);

        walk.AddTransition(StateEnum.Idle, idle);
        walk.AddTransition(StateEnum.Spin, spin);

        spin.AddTransition(StateEnum.Idle, idle);

        stateList.Add(idle);
        stateList.Add(walk);
        stateList.Add(spin);

        for (int i = 0; i < stateList.Count; i++)
        {
            stateList[i].Initialize(move, look, attack);
        }

        _fsm.SetInit(idle);
    }
    private void Update()
    {
        if (InputManager.GetKeyAttack())
        {
            _fsm.Transition(StateEnum.Spin);
        }
        _fsm.OnExecute();
    }
}
