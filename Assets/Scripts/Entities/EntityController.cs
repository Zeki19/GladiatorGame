using System.Collections.Generic;
using Entities.Interfaces;
using Entities.StateMachine;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

namespace Entities
{
    public abstract class EntityController : MonoBehaviour
    {
        public  Stats a;
        [SerializeField] protected EntityManager manager;
        protected FSM<StateEnum> Fsm;
        protected virtual void Update()
        {
            Fsm.OnExecute();
        }
        protected abstract void InitializeFsm();
        
        /// <summary>
        /// Initializes components in the states.
        /// </summary>
        /// <param name="stateList">A List of the states that need to be initialized.</param>
        protected virtual void InitializeComponents(List<State<StateEnum>> stateList)
        {
            var move = GetComponent<IMove>();
            var look = GetComponent<ILook>();
            var attack = GetComponent<IAttack>();
            var sound = GetComponent<ISound>();
            var animate = GetComponent<IAnimate>();
            foreach (var t in stateList)
            {
                t.Initialize(move, look, attack,sound,animate);
            }
        }
    }
}
