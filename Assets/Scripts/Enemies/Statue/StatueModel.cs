using Entities;
using UnityEngine;

namespace Enemies.Statue
{
    public class StatueModel : EntityModel
    {
        [SerializeField] private float attackRange;
        public float AttackRange => attackRange;
        public override void Move(Vector2 dir)
        {
            manager.Rb.linearVelocity =  dir * (moveSpeed);
        }
        public override void ModifySpeed(float speed) { throw new System.NotImplementedException(); }

        public override void Dash(float dashForce) { throw new System.NotImplementedException(); }
    }
}
