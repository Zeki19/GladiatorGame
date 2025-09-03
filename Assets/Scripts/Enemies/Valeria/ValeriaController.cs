using System.Collections.Generic;
using Attack;
using Enemies.Valeria.States;
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

        [SerializeField] private GameObject weapon;
        [SerializeField] private AttackManager attackManager;
        [SerializeField] private LayerMask hiddingLayer;
        [SerializeField] private float hiddingTime;
        [SerializeField] private float invisibilitySpeed;
        [SerializeField] private GameObject stepPrefab;
        public BehaviorGraphAgent agent;
        #region Private Variables

        private StatesBase<EnemyStates> _runAwayState;
        private StatesBase<EnemyStates> _chaseState;
        private StatesBase<EnemyStates> _invisibilityState;
        private StatesBase<EnemyStates> _dashState;
        private StatesBase<EnemyStates> _AttackState;

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


            var dashState = new ValeriaStateDash<EnemyStates>();
            var attackState = new ValeriaStateAttack<EnemyStates>(_pursuitSteering, weapon, attackManager,this);
            var chaseState = new ValeriaStateChase<EnemyStates>(_pursuitSteering, target, desiredDistance, stoppingThreshold, orbitSpeed, orbitAngle, cooldown);
            var runAwayState = new ValeriaStateRunAway<EnemyStates>(target, hiddingLayer, hiddingTime);
            var invisibilityState = new ValeriaStateInvisibility<EnemyStates>(target, invisibilitySpeed, stepPrefab);

            _dashState = dashState;
            _chaseState = chaseState;
            _AttackState = attackState;
            _runAwayState = runAwayState;
            _invisibilityState = invisibilityState;

            var stateList = new List<State<EnemyStates>>
            {
                chaseState,
                dashState,
                attackState,
                runAwayState,
                invisibilityState,
            };
            
            chaseState.AddTransition(EnemyStates.Attack, attackState);
            chaseState.AddTransition(EnemyStates.Dash, dashState);
            chaseState.AddTransition(EnemyStates.RunAway, runAwayState);
            chaseState.AddTransition(EnemyStates.Invisibility, invisibilityState);

            attackState.AddTransition(EnemyStates.Chase, chaseState);
            attackState.AddTransition(EnemyStates.Dash, dashState);
            attackState.AddTransition(EnemyStates.RunAway, runAwayState);
            attackState.AddTransition(EnemyStates.Invisibility, invisibilityState);

            dashState.AddTransition(EnemyStates.Chase, chaseState);
            dashState.AddTransition(EnemyStates.Attack, attackState);
            dashState.AddTransition(EnemyStates.RunAway, runAwayState);
            dashState.AddTransition(EnemyStates.Invisibility, invisibilityState);
            
            runAwayState.AddTransition(EnemyStates.Chase, chaseState);
            runAwayState.AddTransition(EnemyStates.Attack, attackState);
            runAwayState.AddTransition(EnemyStates.RunAway, dashState);
            runAwayState.AddTransition(EnemyStates.Invisibility, invisibilityState);
            
            invisibilityState.AddTransition(EnemyStates.Chase, chaseState);
            invisibilityState.AddTransition(EnemyStates.Attack, attackState);
            invisibilityState.AddTransition(EnemyStates.RunAway, runAwayState);
            invisibilityState.AddTransition(EnemyStates.Invisibility, dashState);

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
