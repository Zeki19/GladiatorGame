using System;
using Entities;
using Entities.Interfaces;
using UnityEngine;

namespace Enemies.Dummy
{
    public class DummyModel : EntityModel, IKnockbackable
    {
        [Header("Movement Settings")]
        private float _speedMult = 1f;
        
        [Header("Attack Settings")]
        [SerializeField] private float damage;
        [SerializeField] private float attackRange;
        
        Action _onAttack = delegate { };
        [Min(0.1f)][SerializeField] private float knockbackWeight=1f;
        private bool _canBeKnockedBack = true;
        public override Action OnAttack { get => _onAttack; set => _onAttack = value; }

        #region Public variables
        public float MoveSpeed => moveSpeed;
        public float AttackRange => attackRange;

        #endregion

        public void Attack()
        { 
            _onAttack();
        }

        #region Movement methods
        
        //Sets the speedMult
        public override void ModifySpeed(float speedMult)
        {
            _speedMult = speedMult;
        }
        
        //Called to Move the char
        public override void Move(Vector2 dir)
        {
            if (_speedMult <= 0) _speedMult = 1;
            manager.Rb.linearVelocity =  dir * (moveSpeed * _speedMult);
        }
        
        //Not implemented for dummy
        public override void Dash(float dashForce)
        {
            throw new NotImplementedException();
        }
        
        #endregion

        #region Utility methods
        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
        #endregion

        #region KnockBack

        float IKnockbackable.KnockbackWeight => knockbackWeight;

        bool IKnockbackable.CanBeKnockedBack => _canBeKnockedBack;

        public void ApplyKnockback(Vector2 force)
        {
            if (!_canBeKnockedBack) return;
            Vector2 adjustedForce = force / knockbackWeight;
            manager.Rb.linearVelocity = Vector2.zero;
            manager.Rb.AddForce(adjustedForce, ForceMode2D.Impulse);
        }

        public void ApplyKnockbackFromSource(Vector2 sourcePosition, float intensity)
        {
            if (!_canBeKnockedBack) return;
            Vector2 direction = (transform.position - (Vector3)sourcePosition).normalized;
            Vector2 force = direction * intensity;
            ApplyKnockback(force);
        }
        #endregion
        }
}
