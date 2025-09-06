using System.Collections.Generic;
using Enemies.Dummy.States;
using Entities.StateMachine;
using Unity.Behavior;
using UnityEngine;

namespace Enemies.Dummy
{
    public class DummyController : EnemyController
    {
        [SerializeField] public DummyStatsSO stats;
        public BehaviorGraphAgent agent;

        #region Private Variables

        private StatesBase<EnemyStates> _idleState; // BLUE

        #endregion

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
            manager.HealthComponent.OnDamage += CheckPhase;
            manager.HealthComponent.OnDead += Die;
        }

        protected override void InitializeFsm()
        {
            Fsm = new FSM<EnemyStates>();

            var idleState = new DummyStateIdle<EnemyStates>();

            _idleState = idleState;

            var stateList = new List<State<EnemyStates>>
            {
                idleState
            };

            // El dummy solo permanece en idle, sin transiciones

            InitializeComponents(stateList);
            Fsm.SetInit(idleState, EnemyStates.Idle);
        }

        private void OnDrawGizmos()
        {
            if (stats == null) return;

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, 1f); // Dummy visual indicator
        }

        private void Die()
        {
            Destroy(gameObject);
        }

        protected override void Update()
        {
            base.Update();
            // El dummy no cambia de fase, pero mantenemos la estructura
            if (agent != null && manager.HealthComponent.currentHealth > 50)
                agent.SetVariableValue("CurrentPhase", global::CurrentPhase.Phace1);
            else if (agent != null)
            {
                agent.SetVariableValue("CurrentPhase", global::CurrentPhase.Phace2);
            }
        }
    }
}