using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;
public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rigidbody;

    private FSM<StateEnum> _fsm;
    private PlayerInput _playerInput;

    private InputAction _moveAction;
    private InputAction _attackAction;
    private InputAction _dashAction;
    private void Awake()
    {
        InitializeFSM();

        _playerInput = GetComponent<PlayerInput>();
        var actionMap = _playerInput.actions.FindActionMap("Player");

        _moveAction = actionMap.FindAction("Move");
        _attackAction = actionMap.FindAction("Attack");
        _dashAction = actionMap.FindAction("Dash");

        _moveAction.performed += ctx => _fsm.HandleMove(ctx.ReadValue<Vector2>());
        _moveAction.canceled += ctx => _fsm.HandleMove(Vector2.zero);
        _attackAction.performed += ctx => _fsm.HandleAttack();
        _dashAction.performed += ctx => _fsm.HandleDash();
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
        var dash = new PSDash<StateEnum>(StateEnum.Idle);

        idle.AddTransition(StateEnum.Walk, walk);
        idle.AddTransition(StateEnum.Spin, spin);
        idle.AddTransition(StateEnum.Dash, dash);

        walk.AddTransition(StateEnum.Idle, idle);
        walk.AddTransition(StateEnum.Spin, spin);
        walk.AddTransition(StateEnum.Dash, dash);


        spin.AddTransition(StateEnum.Idle, idle);

        dash.AddTransition(StateEnum.Idle, idle);
        dash.AddTransition(StateEnum.Walk, walk);
        dash.AddTransition(StateEnum.Spin, spin);

        stateList.Add(idle);
        stateList.Add(walk);
        stateList.Add(spin);
        stateList.Add(dash);

        for (int i = 0; i < stateList.Count; i++)
        {
            stateList[i].Initialize(move, look, attack);
        }

        _fsm.SetInit(idle);
    }
    private void Update()
    {
        _fsm.OnExecute();
    }
    private void FixedUpdate()
    {
        _fsm.OnFixedExecute();
    }
    public void ChangeToAttack()
    {
        _fsm.Transition(StateEnum.Spin);
    }
    public void ChangeToDash()
    {
        _fsm.Transition(StateEnum.Dash);
    }

}
