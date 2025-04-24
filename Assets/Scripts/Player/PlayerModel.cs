using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerModel : MonoBehaviour, IMove, IAttack
{
    [SerializeField] private float _moveSpeed;
    private Rigidbody2D _rb;
    private Vector2 _moveInput;
    private float _speedModifier = 1;
    private Vector2 _lastDirection;
    private PlayerInput _playerInput;
    private InputAction _direction;


    Action _onAttack = delegate { };
    public Action OnAttack { get => _onAttack; set => _onAttack = value; }

    public Vector2 Position => transform.position;

    public float AttackRange => 0;

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        var actionMap = _playerInput.actions.FindActionMap("Player");
        _direction = actionMap.FindAction("Move");

    }
    public virtual void Attack()
    {
        _onAttack();
    }

    public void ModifySpeed(float speed)
    {
        _speedModifier = speed;
    }

    public void Dash(float dashForce)
    {
        _rb.linearVelocity = _lastDirection * (dashForce * _speedModifier);
    }

    public virtual void Move(Vector2 dir)
    {
        _moveInput = _direction.ReadValue<Vector2>();
        if (_moveInput != Vector2.zero) _lastDirection = _moveInput;
        _rb.linearVelocity = _moveInput * (_moveSpeed * _speedModifier);
    }
}
