using System;
using Entities.Interfaces;
using UnityEngine;
using Weapons;
using Weapons.Attacks;

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

        public virtual void SetLinearVelocity(Vector2 velocity)
        {
            manager.Rb.linearVelocity = velocity;
        }
        public void StartAttack(Attack attack,Weapon weapon) => attack.StartAttack(weapon);
        public void ExecuteAttack(Attack attack,Weapon weapon) => attack.ExecuteAttack(weapon);
        public void FinishAttack(Attack attack,Weapon weapon) => attack.FinishAttack(weapon);
    
        public virtual Action OnAttack { get; set; }
    }
}
