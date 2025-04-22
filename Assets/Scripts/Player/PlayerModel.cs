using System;
using Interfaces;
using UnityEngine;

namespace Player
{
    public class PlayerModel : MonoBehaviour, IMove, IAttack
    {

        private Rigidbody2D _rb;
        private Vector2 _moveInput;
        private float _speedModifier = 1;
        private Vector2 _lastDirection;


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
            _rb.linearVelocity = _lastDirection * (dashForce * _speedModifier);
        }

        public virtual void Move(Vector2 dir, float moveSpeed)
        {
            _moveInput = dir;
            if (_moveInput != Vector2.zero) _lastDirection = _moveInput;
            _rb.linearVelocity = _moveInput * (moveSpeed * _speedModifier);
        }
    }
}
