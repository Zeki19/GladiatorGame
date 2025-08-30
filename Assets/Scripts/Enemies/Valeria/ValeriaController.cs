using System.Collections.Generic;
using Attack;
using Enemies.Gaius.States;
using Entities.StateMachine;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemies.Valeria
{
    public class ValeriaController : EnemyController
    {
        [SerializeField] public GaiusStatsSO stats;
        #region Chase Variables
        [SerializeField] private float desiredDistance;
        [SerializeField] private float stoppingThreshold;
        [SerializeField] private float orbitSpeed;
        [SerializeField] private float orbitAngle;
        [SerializeField] private float cooldown;
        #endregion

        public AttackManager attackManager;
        public BehaviorGraphAgent agent;
        #region Private Variables

        private StatesBase<EnemyStates> _idleState; // BLUE
        private StatesBase<EnemyStates> _dashState; // BLUE
        private StatesBase<EnemyStates> _chaseState; // WHITE
        public StatesBase<EnemyStates> _AttackState; // YELLOW

        private ISteering _pursuitSteering;

        #endregion

        protected override void Awake()
        {
            InitalizeSteering();
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
            manager.HealthComponent.OnDamage += CheckPhase;
            manager.HealthComponent.OnDead += Die;
        }

        void InitalizeSteering()
        {
            _pursuitSteering = new StPursuit(transform, target, 0);
        }

        protected override void InitializeFsm()
        {
            Fsm = new FSM<EnemyStates>();

            var chaseState = new States.ValeriaStateChase<EnemyStates>(_pursuitSteering, target, desiredDistance, stoppingThreshold, orbitSpeed, orbitAngle, cooldown);

            _chaseState = chaseState;


            var stateList = new List<State<EnemyStates>>
            {
                chaseState,
            };

            InitializeComponents(stateList);
            Fsm.SetInit(chaseState, EnemyStates.Chase);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, stats.mediumRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, stats.longRange);
            Gizmos.color = Color.green;
            Vector3 origin = transform.position;
            Vector3 direction = transform.up * 5;

            Gizmos.DrawLine(origin, origin + direction);
        }

        private void Die()
        {
            Destroy(gameObject);
            SceneChanger.Instance.ChangeScene(2);
        }
        protected override void Update()
        {
            base.Update();
            if (manager.HealthComponent.currentHealth > 50)
                agent.SetVariableValue("CurrentPhase", global::CurrentPhase.Phase1);
            else
            {
                agent.SetVariableValue("CurrentPhase", global::CurrentPhase.Phase2);
            }
        }
    }
}
