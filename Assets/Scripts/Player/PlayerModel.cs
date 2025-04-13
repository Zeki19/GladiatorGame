using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour, IMove, IAttack
{
    [SerializeField] public float moveSpeed = 5f;

    private Rigidbody2D _rb;
    private Vector2 _moveInput;
    private float _speedModifier = 1;

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

    public virtual void Move(Vector2 dir)
    {
        _moveInput = dir;
        _rb.linearVelocity = _moveInput * moveSpeed * _speedModifier;
    }
}
