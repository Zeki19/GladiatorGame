using System;
using Entities;
using Entities.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public sealed class PlayerModel : EntityModel ,IKnockbackable
    {
        private Vector2 _moveInput;
        private Vector2 _lastDirection;
        private PlayerInput _playerInput;
        private InputAction _direction;

        #region KnockBackVariables

        [SerializeField] private float knockbackWeight;
        private bool _canBeKnockedBack;
        
        #endregion 
        
        Action _onAttack = delegate { };
       
        
        public override Action OnAttack { get => _onAttack; set => _onAttack = value; }

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            var actionMap = _playerInput.actions.FindActionMap("Player");
            _direction = actionMap.FindAction("Move");
        }
        public void Attack()
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

        #region KnockBack

        float IKnockbackable.KnockbackWeight => knockbackWeight;

        bool IKnockbackable.CanBeKnockedBack => _canBeKnockedBack;
        
        public void ApplyKnockbackFromSource(Vector2 sourcePosition, float intensity)
        {
            if (!_canBeKnockedBack) return;
            Vector2 direction = (transform.position - (Vector3)sourcePosition).normalized;
            Vector2 force = direction * intensity;
            ApplyKnockback(force);
        }
        public void ApplyKnockback(Vector2 force)
        {
            if (!_canBeKnockedBack) return;
            Vector2 adjustedForce = force / knockbackWeight;
            manager.Rb.linearVelocity = Vector2.zero;
            manager.Rb.AddForce(adjustedForce, ForceMode2D.Impulse);
        }
        public void SetKnockbackEnabled(bool can) =>_canBeKnockedBack = can;

        #endregion
    }
}
