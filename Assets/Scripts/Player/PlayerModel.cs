using System;
using Entities;
using Entities.Interfaces;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
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

        private PlayerStats _stats;
        
        Action _onAttack = delegate { };


        public override Action OnAttack { get => _onAttack; set => _onAttack = value; }

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            var actionMap = _playerInput.actions.FindActionMap("Player");
            _direction = actionMap.FindAction("Move");
            var playerManager = manager as PlayerManager;
            if (playerManager != null) _stats = playerManager.stats;
            else Debug.LogWarning("manager is not A player manager and it should be");
        }
        public void Attack()
        {
            _onAttack();
        }

       
        public override void ModifySpeed(float speed)
        {
            _stats.SpeedModifier += speed;  //Ask the designer if this should be a + or a *.
        }

        public override void Dash(float dashForce)
        {
            manager.Rb.linearVelocity = _lastDirection * (dashForce *_stats.SpeedModifier);
        }

        public override void Move(Vector2 dir)
        {
            if (IsPlayerTryingToMove()) _lastDirection = _moveInput;
            manager.Rb.linearVelocity = _moveInput * (_stats.MoveSpeed * _stats.SpeedModifier);
        }

        #region KnockBack
        
        public void ApplyKnockbackFromSource(Vector2 sourcePosition, float intensity)
        {
            if (!_stats.canBeKnockedBack) return;
            Vector2 direction = (transform.position - (Vector3)sourcePosition).normalized;
            Vector2 force = direction * intensity;
            ApplyKnockback(force);
        }
        float IKnockbackable.KnockbackWeight => _stats.KnockbackWeight;
        bool IKnockbackable.CanBeKnockedBack => _stats.canBeKnockedBack;
        public void ApplyKnockback(Vector2 force)
        {
            if (!_stats.canBeKnockedBack) return;
            Vector2 adjustedForce = force / _stats.KnockbackWeight;
            manager.Rb.linearVelocity = Vector2.zero;
            manager.Rb.AddForce(adjustedForce, ForceMode2D.Impulse);
        }

        public bool IsPlayerTryingToMove()
        {
            _moveInput = _direction.ReadValue<Vector2>();
            return _moveInput != Vector2.zero;
        }
        public void SetKnockbackEnabled(bool can) =>_stats.canBeKnockedBack = can;

        #endregion
    }
}
