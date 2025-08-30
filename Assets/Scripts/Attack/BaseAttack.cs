using System;
using System.Collections;
using Enemies;
using Entities;
using Entities.Interfaces;
using UnityEngine;

namespace Attack
{
    [Serializable]
    public abstract class BaseAttack : ScriptableObject
    {
        public string attackName;
        public float damage;
        protected GameObject Weapon;
        protected MonoBehaviour CoroutineRunner;
        protected IMove Move;
        protected ILook Look;
        protected IStatus Status;
        protected ITarget Target;
        public event Action AttackFinish;
        public event Action OnHit;


        public abstract void ExecuteAttack();

        public virtual void SetUp(GameObject weapon, IMove move, ILook look, IStatus status,
            MonoBehaviour coroutineRunner,ITarget target)
        {
            Weapon = weapon;
            Move = move;
            Look = look;
            Status = status;
            CoroutineRunner = coroutineRunner;
            Target = target;
        }

        protected void FinishAttack()
        {
            AttackFinish?.Invoke();
        }
        protected void Hit()
        {
            OnHit?.Invoke();
        }

        public virtual void OnUnequip()
        {
            Weapon = null;
            Move = null;
            Look = null;
            Status = null;
            CoroutineRunner = null;
        }

        protected abstract IEnumerator Attack();
    }
}