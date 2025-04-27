using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;
using Interfaces;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rigidbody;
    [SerializeField] private float _dashforce;
    [SerializeField] private float _dashDuration;
    [SerializeField] private float _dashCooldown;
    [SerializeField] private float _dashInvincibility;

    private FSM<StateEnum> _fsm;
    private IHealth _playerHealth;
    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _attackAction;
    private InputAction _dashAction;

    void Dead()
    {
        gameObject.SetActive(false);
    }
    private void Awake()
    {
        InitializeFSM();
        
        _playerHealth = GetComponent<HealthSystem>();

        _playerHealth.OnDead += Dead;
            
        _playerInput = GetComponent<PlayerInput>();
        var actionMap = _playerInput.actions.FindActionMap("Player");

        _moveAction = actionMap.FindAction("Move");
        _attackAction = actionMap.FindAction("Attack");
        _dashAction = actionMap.FindAction("Dash");
    }

    private void InitializeFSM()
    {
        _fsm = new FSM<StateEnum>();
        var move = GetComponent<IMove>();
        var look = GetComponent<ILook>();
        var attack = GetComponent<IAttack>();

        var stateList = new List<PSBase<StateEnum>>();

        var idleState = new PSIdle<StateEnum>();
        var walkState = new PSWalk<StateEnum>();
        var attackState = new PSAttack<StateEnum>(StateEnum.Idle);
        var dashState = new PSDash<StateEnum>(StateEnum.Idle, _dashforce, _dashDuration, _dashCooldown, _dashInvincibility, _playerHealth, this);

        idleState.AddTransition(StateEnum.Walk, walkState);
        idleState.AddTransition(StateEnum.Attack, attackState);
        idleState.AddTransition(StateEnum.Dash, dashState);

        walkState.AddTransition(StateEnum.Idle, idleState);
        walkState.AddTransition(StateEnum.Attack, attackState);
        walkState.AddTransition(StateEnum.Dash, dashState);


        attackState.AddTransition(StateEnum.Idle, idleState);

        dashState.AddTransition(StateEnum.Idle, idleState);
        //dashState.AddTransition(StateEnum.Walk, walkState);

        stateList.Add(idleState);
        stateList.Add(walkState);
        stateList.Add(attackState);
        stateList.Add(dashState);

        for (int i = 0; i < stateList.Count; i++)
        {
            stateList[i].Initialize(move, look, attack);
        }

        _fsm.SetInit(idleState);
    }
    private void Update()
    {
        _fsm.OnExecute();
    }
    private void FixedUpdate()
    {
        _fsm.OnFixedExecute();
    }

    public void ChangeToMove()
    {
        _fsm.Transition(StateEnum.Walk);
    }
    public void ChangeToAttack()
    {
        _fsm.Transition(StateEnum.Attack);
    }
    public void ChangeToDash()
    {
        _fsm.Transition(StateEnum.Dash);
    }

}
