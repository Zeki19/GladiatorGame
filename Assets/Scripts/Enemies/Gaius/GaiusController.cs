using System.Collections.Generic;
using Enemies.FirstBossTest.States;
using Enemies.Hounds.States;
using Entities;
using Entities.Interfaces;
using Entities.StateMachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemies.FirstBossTest
{
    public class GaiusController : EnemyController
    {
        [SerializeField] int[] phasesThresholds;

        public SpriteRenderer SpriteRendererBoss;

        #region Private Variables


        #endregion

        private PhaseSystem _phaseSystem;
        private int _currentPhase = 1;
        
        protected override void Awake()
        {
            SpriteRendererBoss = GetComponent<SpriteRenderer>();
        }

        protected override void Start()
        {
            base.Start();
            _phaseSystem = new PhaseSystem(phasesThresholds, manager.HealthComponent);
            manager.HealthComponent.OnDamage += CheckPhase;
            manager.HealthComponent.OnDead += Die;
        }
        protected override void InitializeFsm()
        {
            Fsm = new FSM<StateEnum>();

            var move = GetComponent<IMove>();
            var look = GetComponent<ILook>();
            var attack = GetComponent<IAttack>();
            /*
            var idleState = new FirstBossStateIdle<StateEnum>(this, idleDuration, SpriteRendererBoss);
            var chaseState = new FirstBossStateChase<StateEnum>(_leaderSteering, _avoidWalls, transform,target.transform,attackRange, GetComponent<LeaderBehaviour>(), flockMask, SpriteRendererBoss);
            var attackState = new FirstBossStateAttack<StateEnum>(target.transform, _attacks, _lowHealthAttacks, this, AttackCooldown, SpriteRendererBoss, chompEffect);
            var patrolState = new FirstBossStatePatrol<StateEnum>(this.transform, patrolDuration, camp.GetPoints(amountOfWaypoints), this, SpriteRendererBoss);
            var searchState = new FirstBossStateSearch<StateEnum>(_toPointSteering, _avoidWalls, manager.model.transform, this, SpriteRendererBoss);
            var runAwayState = new FirstBossStateRunAway<StateEnum>(this.transform, camp.transform, this, manager, SpriteRendererBoss);
            
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
            
            attackState.AddTransition(StateEnum.Chase, chaseState);
            attackState.AddTransition(StateEnum.Runaway, runAwayState);

            foreach (var t in stateList)
            {
                t.Initialize(move, look, attack);
            }

            Fsm.SetInit(idleState,StateEnum.Idle);
            */
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

        private void Die()
        {
            Destroy(gameObject);
        }

    }
}