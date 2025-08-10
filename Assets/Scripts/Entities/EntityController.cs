using System.Collections.Generic;
using Enemies;
using Entities.Interfaces;
using Entities.StateMachine;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Analytics;

namespace Entities
{
    public abstract class EntityController : MonoBehaviour
    {
        [SerializeField] protected EntityManager manager;
        
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
            var Agent = GetComponent<INavigation>();

            foreach (var t in stateList)
            {
                t.Initialize(move, look, attack,sound,animate,Status,Conditions,StateData, Agent);
            }
        }
    }
}
