using System;
using Entities;
using Interfaces;
using UnityEngine;

namespace Dummy
{
    public class DummyModel : EntityModel
    {
        [Header("Movement Settings")]
        private float _speedMult = 1f;
        
        [Header("Attack Settings")]
        [SerializeField] private float damage;
        [SerializeField] private float attackRange;
        
        Action _onAttack = delegate { };
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
    }
}
