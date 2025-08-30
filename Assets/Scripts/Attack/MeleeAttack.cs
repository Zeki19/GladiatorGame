using Enemies;
using Entities;
using Entities.Interfaces;
using UnityEngine;

namespace Attack
{
    public abstract class MeleeAttack : BaseAttack
    {
        protected Collider2D collider;
        public override void SetUp(GameObject weapon, IMove move, ILook look, IStatus status, MonoBehaviour coroutineRunner,ITarget target)
        {
            base.SetUp(weapon, move, look, status, coroutineRunner,target);
            collider=weapon.GetComponent<Collider2D>();
        }
    }
}