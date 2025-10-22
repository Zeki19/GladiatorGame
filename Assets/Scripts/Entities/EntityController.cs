using System;
using System.Collections.Generic;
using Enemies;
using Entities.Interfaces;
using UnityEngine;

namespace Entities
{
    public abstract class EntityController : MonoBehaviour, IStatus ,ITarget
    {
        [SerializeField] protected EntityManager manager;
        [SerializeField] protected EntityManager _target;

        protected abstract void InitializeFsm();

        /// <summary>
        /// Initializes components in the states.
        /// </summary>
        /// <param name="stateList">A List of the states that need to be initialized.</param>
        protected virtual void InitializeComponents<T>(List<State<T>> stateList)
        {
            var move = GetComponent<IMove>();
            var look = GetComponent<ILook>();
            var attack = GetComponent<IAttack>();
            var sound = GetComponent<ISound>();
            var animate = GetComponent<IAnimate>();
            var Status = GetComponent<IStatus>();
            var Conditions = GetComponent<ICondition>();
            var StateData = GetComponent<IStatesData>();
            var Target = GetComponent<ITarget>();
            var Agent = GetComponent<INavigation>();
            foreach (var t in stateList)
            {
                t.Initialize(move, look, attack, sound, animate, Status, Conditions, StateData, Target, Agent);
            }
        }

        protected virtual void Awake()
        {
            //InitializeFsm();
            foreach (StatusEnum status in (StatusEnum[]) Enum.GetValues(typeof(StatusEnum)))
            {
                Status.Add(status, false);
            }
        }

        #region Status

        protected Dictionary<StatusEnum, bool> Status = new Dictionary<StatusEnum, bool>();

        public bool GetStatus(StatusEnum status)
        {
            return Status[status];
        }

        public void SetStatus(StatusEnum status, bool value)
        {
            Status[status] = value;
        }

        #endregion

        public virtual EntityManager GetTarget()
        {
            return _target;
        }

        public virtual void SetTarget(EntityManager Target)
        {
            _target=Target;
        }
    }
}
