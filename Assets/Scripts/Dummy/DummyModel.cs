using System;
using Interfaces;
using UnityEngine;

namespace Dummy
{
    public class DummyModel : MonoBehaviour, IMove, IAttack
    {
        [Header("Rigidbody2D Ref")]
        [SerializeField] private Rigidbody2D rb;
        
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5f;
        private float _speedMult = 1f;
        
        [Header("Attack Settings")]
        [SerializeField] private float damage;
        [SerializeField] private float attackRange;
        
        Action _onAttack = delegate { };
        public Action OnAttack { get => _onAttack; set => _onAttack = value; }

        #region Public variables

        public float MoveSpeed => moveSpeed;
        public float AttackRange => attackRange;
        public Vector2 Position => transform.position;

        #endregion


        public void Attack()
        { 
            _onAttack();
        }

        #region Movement methods
        
        //Sets the speedMult
        public void ModifySpeed(float speedMult)
        {
            _speedMult = speedMult;
        }
        
        //Called to Move the char
        public void Move(Vector2 dir)
        {
            if (_speedMult <= 0) _speedMult = 1;
            rb.linearVelocity =  dir * (moveSpeed * _speedMult);
        }
        
        //Not implemented for dummy
        public void Dash(float dashForce)
        {
            throw new NotImplementedException();
        }
        
        //Sets the linearVelocity to Vector2.Zero
        public void StopMovement()
        {
            rb.linearVelocity = Vector2.zero;
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
