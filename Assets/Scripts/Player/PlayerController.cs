using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;
public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rigidbody;
    [SerializeField] float _moveSpeed;
    [SerializeField] float _dashforce;
    [SerializeField] float _dashDuration;
    [SerializeField] float _dashCooldown;
    [SerializeField] private float _dashInvincibility;

    private FSM<StateEnum> _fsm;
    private IHealth _playerHealth;
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
        _playerHealth = new HealthSystem(100);
    }

    private void InitializeFSM()
    {
        _fsm = new FSM<StateEnum>();
        var move = GetComponent<IMove>();
        var look = GetComponent<ILook>();
        var attack = GetComponent<IAttack>();

        var stateList = new List<PSBase<StateEnum>>();

        var idleState = new PSIdle<StateEnum>(StateEnum.Walk);
        var walkState = new PSWalk<StateEnum>(StateEnum.Idle, _moveSpeed);
        var attackState = new PSAttack<StateEnum>(StateEnum.Idle, _moveSpeed);
        var dashState = new PSDash<StateEnum>(StateEnum.Idle, _dashforce, _dashDuration, _dashCooldown, _dashInvincibility, _playerHealth, this);

        idleState.AddTransition(StateEnum.Walk, walkState);
        idleState.AddTransition(StateEnum.Attack, attackState);
        idleState.AddTransition(StateEnum.Dash, dashState);

        walkState.AddTransition(StateEnum.Idle, idleState);
        walkState.AddTransition(StateEnum.Attack, attackState);
        walkState.AddTransition(StateEnum.Dash, dashState);


        attackState.AddTransition(StateEnum.Idle, idleState);

        dashState.AddTransition(StateEnum.Idle, idleState);
        dashState.AddTransition(StateEnum.Walk, walkState);
        dashState.AddTransition(StateEnum.Attack, attackState);

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
        _fsm.OnExecute(_moveAction.ReadValue<Vector2>());
    }
    private void FixedUpdate()
    {
        _fsm.OnFixedExecute();
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
