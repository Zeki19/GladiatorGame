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
    
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position,attackRange);
        }
    }
}
