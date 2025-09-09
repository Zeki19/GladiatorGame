using System;
using System.Collections;
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
        private PlayerManager _manager;

        private PlayerStats _stats;
        Action _onAttack = delegate { };

        //player events
        public static event Action OnPlayerMoved;
        public static event Action OnPlayerDashed;
        public override Action OnAttack { get => _onAttack; set => _onAttack = value; }

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            var actionMap = _playerInput.actions.FindActionMap("Player");
            _direction = actionMap.FindAction("Move");
            _manager = manager as PlayerManager;
            if (_manager != null) _stats = _manager.stats;
            else Debug.LogWarning("manager is not A player manager and it should be");
            manager.HealthComponent.OnDamage += AtivateInvulnerability;

                ServiceLocator.Instance.RegisterService<PlayerModel>(this);
        }
        public void Attack()
        {
            _onAttack();
        }

        public void AtivateInvulnerability(float not)//Falta agregar una forma de terminar la corutina si esta funcion se vuelve a llamar artes de terminar
        {
            StartCoroutine(PlayerInvulnerability());
        }
        IEnumerator PlayerInvulnerability()
        {
            manager.HealthComponent.isInvulnerable = true;
            yield return new WaitForSeconds(_manager.stats.IFrames);
            manager.HealthComponent.isInvulnerable = false;
        }
       
        public override void ModifySpeed(float speed)
        {
            _stats.SpeedModifier += speed;  //Ask the designer if this should be a + or a *.
        }

        public override void Dash(float dashForce)
        {
            manager.Rb.linearVelocity = _lastDirection * (dashForce *_stats.SpeedModifier);
            OnPlayerDashed?.Invoke();
        }

        public override void Dash(Vector2 dir, float dashForce)
        {
            throw new NotImplementedException();
   
        }

        public override void Dash(Vector2 dir, float dashForce, float backStepDistance)
        {
            throw new NotImplementedException();
        }


        public override void Move(Vector2 dir)
        {
            if (IsPlayerTryingToMove())
            {
                _lastDirection = _moveInput;
                OnPlayerMoved?.Invoke();
            }
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
