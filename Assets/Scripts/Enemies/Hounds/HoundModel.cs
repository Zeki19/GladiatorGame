using System;
using Entities;
using UnityEngine;
using Weapons;

namespace Enemies.Hounds
{
    public class HoundModel : EntityModel
    {
        [Header("Movement Settings")]
        [Tooltip("Speed at which the hound patrols.")]
        [SerializeField] private float amountOfWaypoints = 5f;
        [Header("Attack Settings")]
        [SerializeField] private float attackRange;
        [SerializeField] private float attackCooldown;

        [SerializeField] protected float moveSpeed;
        protected float _speedModifier;
        #region Public calls to private variables
        
        public float AttackRange => attackRange;
        public float AttackCooldown => attackCooldown;
        public float AmountOfWaypoints => amountOfWaypoints;

        #endregion
        
        public override void ModifySpeed(float speed)
        {
            _speedModifier = speed;
        }

        public override void Move(Vector2 dir)
        {
            if (_speedModifier <= 0) _speedModifier = 1;
            manager.Rb.linearVelocity =  dir * (moveSpeed * _speedModifier);
        }

        public override void Dash(float dashForce)
        {
            throw new NotImplementedException();
        }
        
        public void AttackTarget(Transform target, float damage)
        {
            if (target == null) return;

            var health = target.GetComponent<HealthSystem.HealthSystem>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }
    
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position,attackRange);
        }
    }
}
