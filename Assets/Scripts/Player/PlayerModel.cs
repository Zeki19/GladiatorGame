using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour, IMove, IAttack
{

    private Rigidbody2D _rb;
    private Vector2 _moveInput;
    private float _speedModifier = 1;
    private Vector2 _lastDirection;
    private bool _isDashing;
    private float _dashTimer;
    private float _cooldownTimer;


    Action _onAttack = delegate { };
    public Action OnAttack { get => _onAttack; set => _onAttack = value; }

    public Vector2 Position => transform.position;

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
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
        _rb.linearVelocity = _moveInput * dashForce * _speedModifier;
    }

    public virtual void Move(Vector2 dir, float moveSpeed)
    {
        if (dir != Vector2.zero)
        {
            _moveInput = dir.normalized;
        }
        else
        {
            _moveInput = dir;
        }
        _rb.linearVelocity = _moveInput * moveSpeed * _speedModifier;
    }
}
