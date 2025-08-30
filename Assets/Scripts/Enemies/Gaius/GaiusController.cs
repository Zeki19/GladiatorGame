using System.Collections.Generic;
using Attack;
using Enemies.Gaius.States;
using Entities.StateMachine;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Enemies.Gaius
{
    public class GaiusController : EnemyController
    {
        [SerializeField] public GaiusStatsSO stats;
        public GameObject weapon;
        private AttackManager _attackManager;
        private BehaviorGraphAgent _agent;
        #region Private Variables

        private StatesBase<EnemyStates> _idleState; // BLUE
        private StatesBase<EnemyStates> _dashState; // BLUE
        private StatesBase<EnemyStates> _chaseState; // WHITE
        public StatesBase<EnemyStates> _AttackState; // YELLOW

        private ISteering _pursuitSteering;

        #endregion

        protected override void Awake()
        {
            _attackManager = GetComponent<AttackManager>();
            _agent = GetComponent<BehaviorGraphAgent>();
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

            var idleState = new GaiusStateIdle<EnemyStates>();
            var dashState = new GaiusStateDash<EnemyStates>();
            var chaseState = new GaiusStateChase<EnemyStates>(_pursuitSteering, this, target);
            var AttackState = new GaiusStateAttack<EnemyStates>(_pursuitSteering, weapon, _attackManager,this);

            _idleState = idleState;
            _dashState = dashState;
            _chaseState = chaseState;
            _AttackState = AttackState;


            var stateList = new List<State<EnemyStates>>
            {
                idleState,
                dashState,
                chaseState,
                AttackState
            };

            idleState.AddTransition(EnemyStates.Chase, chaseState);
            idleState.AddTransition(EnemyStates.Attack, AttackState);
            idleState.AddTransition(EnemyStates.Dash, dashState);

            chaseState.AddTransition(EnemyStates.Idle, idleState);
            chaseState.AddTransition(EnemyStates.Attack, AttackState);
            chaseState.AddTransition(EnemyStates.Dash, dashState);

            AttackState.AddTransition(EnemyStates.Idle, idleState);
            AttackState.AddTransition(EnemyStates.Chase, chaseState);
            AttackState.AddTransition(EnemyStates.Dash, dashState);

            dashState.AddTransition(EnemyStates.Idle, idleState);
            dashState.AddTransition(EnemyStates.Chase, chaseState);
            dashState.AddTransition(EnemyStates.Attack, AttackState);
            ;

            InitializeComponents(stateList);
            Fsm.SetInit(idleState, EnemyStates.Idle);
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
            if(manager.HealthComponent.currentHealth>50)
                _agent.SetVariableValue("CurrentPhase",global::CurrentPhase.Phase1);
            else
            {
                _agent.SetVariableValue("CurrentPhase", global::CurrentPhase.Phase2);
            }
        }
    }
}

[BlackboardEnum]
public enum EnemyStates
{
    Idle,
    Chase,
    Attack,
    Stunned,
    Patrol,
    Surround,
    Dash
}
