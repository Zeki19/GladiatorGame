using System;
using Interfaces;
using UnityEngine;

namespace Dummy
{
    public class DummyModel : MonoBehaviour, IMove, IAttack
    {
        public float _speed;
        public float attackRange;
        public LayerMask enemyMask;
        public Vector2 Position { get; }
        public float damageModifier { get; set; }
    
        public void Attack()
        {
            var colls = Physics.OverlapSphere(Position, attackRange, enemyMask);
            foreach (var t in colls)
            {
                Destroy(t.gameObject);
            }
        }

        public Action OnAttack { get; set; } = delegate { };


        public void ModifySpeed(float speed)
        {
            _speed = speed;
        }

        public void Move(float moveSpeed)
        {
            transform.position = Vector2.one;
        }
    
        public void Dash(float dashForce)
        {
            throw new NotImplementedException();
        }
        public void ModifyDamage(float damage)
        {
            throw new NotImplementedException();
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }

    }
}
