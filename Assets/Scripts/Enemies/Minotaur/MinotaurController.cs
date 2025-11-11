using System.Collections.Generic;
using Attack;
using Enemies.Gaius.States;
using Enemies.Minotaur.States;
using Entities.StateMachine;
using Unity.Behavior;
using UnityEngine;

namespace Enemies.Minotaur
{
    public class MinotaurController : EnemyController
    {
        [SerializeField] public GaiusStatsSO stats;

        [SerializeField] private GameObject weapon;
        [SerializeField] private AttackManager attackManager;
        [SerializeField] private float searchSpeed;
        [SerializeField] private GameObject stepPrefab;
        [SerializeField] private float shortAttackCD;
        [SerializeField] private float longAttackCD;
        public BehaviorGraphAgent agent;

        #region Private Variables
        private StatesBase<EnemyStates> _chaseState;
        private StatesBase<EnemyStates> _searchState;
        private StatesBase<EnemyStates> _desperateSearchState;
        private StatesBase<EnemyStates> _attackState;
        private StatesBase<EnemyStates> _deathState;

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
            manager.HealthComponent.OnDamage += ChangePhaseStats;
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
            var attackState = new MinotaurStateAttack<EnemyStates>(_pursuitSteering, weapon, attackManager,this);
            var chaseState = new MinotaurStateChase<EnemyStates>(_pursuitSteering, this,shortAttackCD,longAttackCD);
            var searchState = new MinotaurStateSearch<EnemyStates>(_pursuitSteering,this);
            var desperateSearchState = new MinotaurStateDesperateSearch<EnemyStates>(_pursuitSteering, searchSpeed);
            var deathState = new DeadState<EnemyStates>();


            _chaseState = chaseState;
            _searchState = searchState;
            _desperateSearchState = desperateSearchState;
            _attackState = attackState;
            _deathState = deathState;

            var stateList = new List<State<EnemyStates>>
            {
                chaseState,
                searchState,
                desperateSearchState,
                attackState,
                deathState
            };
            
            chaseState.AddTransition(EnemyStates.Patrol, searchState);
            chaseState.AddTransition(EnemyStates.Surround, desperateSearchState);
            chaseState.AddTransition(EnemyStates.Attack, attackState);
            chaseState.AddTransition(EnemyStates.Death, deathState);
            
            searchState.AddTransition(EnemyStates.Chase, chaseState);
            searchState.AddTransition(EnemyStates.Surround, desperateSearchState);
            searchState.AddTransition(EnemyStates.Death, deathState);
            searchState.AddTransition(EnemyStates.Attack, attackState);

            desperateSearchState.AddTransition(EnemyStates.Chase, chaseState);
            desperateSearchState.AddTransition(EnemyStates.Patrol, searchState);
            desperateSearchState.AddTransition(EnemyStates.Death, deathState);
            desperateSearchState.AddTransition(EnemyStates.Attack, attackState);
            
            attackState.AddTransition(EnemyStates.Chase,chaseState);
            attackState.AddTransition(EnemyStates.Attack, attackState);
            attackState.AddTransition(EnemyStates.Surround,desperateSearchState);
            attackState.AddTransition(EnemyStates.Death, deathState);
            attackState.AddTransition(EnemyStates.Patrol,searchState);

            InitializeComponents(stateList);
            Fsm.SetInit(chaseState, EnemyStates.Chase);
        }

        private void ChangePhaseStats(float filler)
        {
            if (_currentPhase != manager.PhaseSystem.CurrentPhase())
            {
                //_NVagent.speed += stats.SpeedChange;
            }
        }

        private void Die()
        {
            manager.PlaySound("Death");
            PauseManager.OnCinematicStateChanged -= HandlePause;
            ChangeToState(EnemyStates.Death);
            BossExitDoor door = ServiceLocator.Instance.GetService<BossExitDoor>();
            if (door != null)
            {
                door.OnBossDefeated();
            }
        }
    }
}
