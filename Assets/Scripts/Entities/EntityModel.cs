using System;
using Entities.Interfaces;
using UnityEngine;

namespace Entities
{
    public abstract class EntityModel : MonoBehaviour, IMove, IAttack
    {
        [SerializeField] protected EntityManager manager;
        public EntityManager Manager => manager;

        public Vector2 Position => transform.position;

        public void StopMovement() => manager.Rb.linearVelocity = Vector2.zero;
        public abstract void ModifySpeed(float speed);
        public abstract void Move(Vector2 dir);
        public abstract void Dash(float dashForce);
        public abstract void Dash(Vector2 dir, float dashForce);
        public abstract void Dash(Vector2 dir, float dashForce, float backStepDistance);

        public virtual void SetLinearVelocity(Vector2 velocity)
        {
            manager.Rb.linearVelocity = velocity;
        }
    
        public virtual Action OnAttack { get; set; }
    }
}
