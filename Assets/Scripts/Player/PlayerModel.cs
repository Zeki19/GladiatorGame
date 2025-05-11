using System;
using Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerModel : EntityModel
    {
        private Vector2 _moveInput;
        private Vector2 _lastDirection;
        private PlayerInput _playerInput;
        private InputAction _direction;


        Action _onAttack = delegate { };
        public override Action OnAttack { get => _onAttack; set => _onAttack = value; }

        protected virtual void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            var actionMap = _playerInput.actions.FindActionMap("Player");
            _direction = actionMap.FindAction("Move");
        }
        public virtual void Attack()
        {
            _onAttack();
        }

        public override void ModifySpeed(float speed)
        {
            _speedModifier += speed;  //Ask the designer if this should be a + or a *.
        }

        public override void Dash(float dashForce)
        {
            manager.Rb.linearVelocity = _lastDirection * (dashForce * _speedModifier);
        }

        public override void Move(Vector2 dir)
        {
            _moveInput = _direction.ReadValue<Vector2>();
            if (_moveInput != Vector2.zero) _lastDirection = _moveInput;
            manager.Rb.linearVelocity = _moveInput * (moveSpeed * _speedModifier);
        }
    }
}
