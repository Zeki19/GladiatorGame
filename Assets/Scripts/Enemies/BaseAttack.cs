using System;
using System.Collections;
using Entities;
using Entities.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemies
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
        public event Action AttackFinish;


        public abstract void ExecuteAttack();

        public virtual void SetUp(GameObject weapon, IMove move, ILook look, IStatus status,
            MonoBehaviour coroutineRunner)
        {
            Weapon = weapon;
            Move = move;
            Look = look;
            Status = status;
            CoroutineRunner = coroutineRunner;
        }

        protected void FinishAttack()
        {
            AttackFinish?.Invoke();
        }

        protected abstract IEnumerator Attack();
    }
}