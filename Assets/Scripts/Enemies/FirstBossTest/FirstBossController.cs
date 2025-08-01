using System.Collections.Generic;
using Enemies.FirstBossTest.States;
using Enemies.Hounds.States;
using Entities.Interfaces;
using Entities.StateMachine;
using UnityEngine;

namespace Enemies.FirstBossTest
{
    public class FirstBossController : EnemyController
    {

        private StatesBase<StateEnum> _idleState;
        private StatesBase<StateEnum> _chaseState;
        private StatesBase<StateEnum> _attackState;
        private StatesBase<StateEnum> _patrolState;
        private StatesBase<StateEnum> _searchState;
        private StatesBase<StateEnum> _runAwayState;
        public SpriteRenderer SpriteRendererBoss;
        public StatesBase<StateEnum> IdleState => _idleState;
        public StatesBase<StateEnum> ChaseState => _chaseState;
        public StatesBase<StateEnum> AttackState => _attackState; 
        public StatesBase<StateEnum> PatrolState => _patrolState;
        public StatesBase<StateEnum> SearchState => _searchState;
        public StatesBase<StateEnum> RunAwayState => _runAwayState;

        [Header("Required GameObjects")] [SerializeField]
        private HoundsCamp camp;

        [SerializeField] private int amountOfWaypoints;

        [Header("States Settings")] [Tooltip("Time it takes to force a state change.")] [SerializeField]
        private float idleDuration;

        [SerializeField] private float patrolDuration;
        [SerializeField] private ChompEffect chompEffect;
        [SerializeField] private float AttackCooldown;
        [SerializeField] private LayerMask flockMask;

        [Header("Obstacle Avoidance Settings")] [SerializeField]
        public int _maxObs;

        [SerializeField] public float _radius;
        [SerializeField] public float _angle;
        [SerializeField] public float _personalArea;
        [SerializeField] public LayerMask _obsMask;

        private Vector2 _targetLastPos;

        #region Private Variables

        private ISteering _steering;
        private ISteering _patrolSteering;
        private ISteering _pursuitSteering;
        private ISteering _runawaySteering;
        private ISteering _toPointSteering;
        private ISteering _leaderSteering;
        private StObstacleAvoidance _avoidWalls;

        #endregion

        private Dictionary<AttackType, float> _attacks = new Dictionary<AttackType, float>
        {
            { AttackType.Normal, 60f },
            { AttackType.Charge, 30f },
            { AttackType.Lunge, 10f }
        };
        
        private Dictionary<AttackType, float> _lowHealthAttacks = new Dictionary<AttackType, float>
        {
            { AttackType.Normal, 50f },
            { AttackType.Charge, 20f },
            { AttackType.Lunge, 10f },
            { AttackType.Super, 20f }
        };
        
        protected override void Awake()
        {
            SpriteRendererBoss = GetComponent<SpriteRenderer>();
            InitalizeSteering();
            _avoidWalls = new StObstacleAvoidance(_maxObs, _radius, _angle, _personalArea, _obsMask);
            base.Awake();
            objectContext.Points[0] = (camp.CampCenter, camp.patrolRadius);
            objectContext.Points[1] = (camp.CampCenter, camp.chaseRadius*camp.patrolRadius);
        }

        protected override void Start()
        {
            base.Start();
            _phaseSystem = new PhaseSystem(phasesThresholds, manager.HealthComponent);
            manager.HealthComponent.OnDamage += CheckPhase;
            manager.HealthComponent.OnDead += Die;
        }

        void InitalizeSteering()
        {
            _leaderSteering = GetComponent<FlockingManager>();
            _runawaySteering = new StToPoint(camp.CampCenter, manager.model.transform);
            _pursuitSteering = new StPursuit(manager.model.transform, target, 0);
            _toPointSteering = new StToPoint(_targetLastPos, manager.model.transform);
        }
        protected override void InitializeFsm()
        {
            Fsm = new FSM<StateEnum>();

            var move = GetComponent<IMove>();
            var look = GetComponent<ILook>();
            var attack = GetComponent<IAttack>();
            
            var idleState = new FirstBossStateIdle<StateEnum>(this, idleDuration, SpriteRendererBoss);
            //var chaseState = new FirstBossStateChase<StateEnum>(_leaderSteering, _avoidWalls, transform,target.transform,attackRange, GetComponent<LeaderBehaviour>(), flockMask, SpriteRendererBoss);
            var attackState = new FirstBossStateAttack<StateEnum>(target.transform, _attacks, _lowHealthAttacks, this, AttackCooldown, SpriteRendererBoss, chompEffect);
            var patrolState = new FirstBossStatePatrol<StateEnum>(this.transform, patrolDuration, camp.GetPoints(amountOfWaypoints), this, SpriteRendererBoss);
            var searchState = new FirstBossStateSearch<StateEnum>(_toPointSteering, _avoidWalls, manager.model.transform, this, SpriteRendererBoss);
            var runAwayState = new FirstBossStateRunAway<StateEnum>(this.transform, camp.transform, this, manager, SpriteRendererBoss);
            
            _idleState = idleState;
            _attackState = attackState;
            //_chaseState = chaseState;
            _searchState = searchState;
            _patrolState = patrolState;
            _runAwayState = runAwayState;

            var stateList = new List<StatesBase<StateEnum>>
            {
                idleState,
                //chaseState,
                attackState,
                searchState,
                patrolState,
                runAwayState,
            };

            idleState.AddTransition(StateEnum.Patrol, patrolState);
            idleState.AddTransition(StateEnum.Runaway, runAwayState);
            
            patrolState.AddTransition(StateEnum.Idle, idleState);
            //patrolState.AddTransition(StateEnum.Chase, chaseState);

            //chaseState.AddTransition(StateEnum.Search, searchState);
            //chaseState.AddTransition(StateEnum.Runaway, runAwayState);
            //chaseState.AddTransition(StateEnum.Attack, attackState);
            
            //searchState.AddTransition(StateEnum.Chase, chaseState);
            searchState.AddTransition(StateEnum.Runaway, runAwayState);
            
            runAwayState.AddTransition(StateEnum.Idle, idleState);
            runAwayState.AddTransition(StateEnum.Patrol, patrolState);
            
            //attackState.AddTransition(StateEnum.Chase, chaseState);
            attackState.AddTransition(StateEnum.Runaway, runAwayState);

            foreach (var t in stateList)
            {
                t.Initialize(move, look, attack);
            }

            Fsm.SetInit(idleState,StateEnum.Idle);
        }

        protected override void InitializeTree()
        {
            Root.Execute(objectContext);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            //Gizmos.DrawWireSphere(transform.position, attackRange);
        }

        private void Die()
        {
            Destroy(gameObject);
        }

    }
}