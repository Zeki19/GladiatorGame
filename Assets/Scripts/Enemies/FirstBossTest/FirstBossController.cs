using System.Collections.Generic;
using Enemies.FirstBossTest.States;
using Entities.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemies.FirstBossTest
{
    public class FirstBossController : EnemyController
    {
        [SerializeField] int[] phasesThresholds;

        private States_Base<StateEnum> _idleState;
        private States_Base<StateEnum> _chaseState;
        private States_Base<StateEnum> _attackState;
        private States_Base<StateEnum> _patrolState;
        private States_Base<StateEnum> _searchState;
        private States_Base<StateEnum> _runAwayState;

        public States_Base<StateEnum> IdleState => _idleState;
        public States_Base<StateEnum> ChaseState=> _chaseState;
        public States_Base<StateEnum> AttackState=> _attackState;
        public States_Base<StateEnum> PatrolState=> _patrolState;
        public States_Base<StateEnum> SearchState=> _searchState;
        public States_Base<StateEnum> RunAwayState=> _runAwayState;
        
        private PhaseSystem _phaseSystem;
        private int _currentPhase = 1;
        public bool isRested;
        public bool isTired;
        public bool isAttackOnCd;

        private void Start()
        {
            _phaseSystem = new PhaseSystem(phasesThresholds, manager.HealthComponent);
            manager.HealthComponent.OnDamage += CheckPhase;
        }

        protected override void InitializeFsm()
        {
            Fsm = new FSM<StateEnum>();

            var move = GetComponent<IMove>();
            var look = GetComponent<ILook>();
            var attack = GetComponent<IAttack>();

            var idleState = new FirstBossStateIdle<StateEnum>();
            var chaseState = new FirstBossStateChase<StateEnum>();
            var attackState = new FirstBossStateAttack<StateEnum>();
            var patrolState = new FirstBossStatePatrol<StateEnum>();
            var searchState = new FirstBossStateSearch<StateEnum>();
            var runAwayState = new FirstBossStateRunAway<StateEnum>();
            
            _idleState = idleState;
            _attackState = attackState;
            _chaseState = chaseState;
            _searchState = searchState;
            _patrolState = patrolState;
            _runAwayState = runAwayState;

            var stateList = new List<States_Base<StateEnum>>
            {
                idleState,
                chaseState,
                attackState,
                searchState,
                patrolState,
                runAwayState,
            };

            idleState.AddTransition(StateEnum.Patrol, patrolState);
            idleState.AddTransition(StateEnum.Runaway, runAwayState);
            
            patrolState.AddTransition(StateEnum.Idle, idleState);
            patrolState.AddTransition(StateEnum.Chase, chaseState);

            chaseState.AddTransition(StateEnum.Search, searchState);
            chaseState.AddTransition(StateEnum.Runaway, runAwayState);
            chaseState.AddTransition(StateEnum.Attack, attackState);
            
            searchState.AddTransition(StateEnum.Chase, chaseState);
            searchState.AddTransition(StateEnum.Runaway, runAwayState);
            
            runAwayState.AddTransition(StateEnum.Idle, idleState);
            runAwayState.AddTransition(StateEnum.Patrol, patrolState);

            attackState.AddTransition(StateEnum.Idle, idleState);

            foreach (var t in stateList)
            {
                t.Initialize(move, look, attack);
            }

            Fsm.SetInit(idleState);
        }

        protected override void InitializeTree()
        {
            Root.Execute(objectContext);
        }

        void CheckPhase(float damage)
        {
            _currentPhase = _phaseSystem.currentPhase();
            Debug.Log("Current phase is:" + _currentPhase);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}