using System.Collections.Generic;
using Attack;
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
        public BehaviorGraphAgent agent;

        #region Private Variables
        private StatesBase<EnemyStates> _chaseState;
        private StatesBase<EnemyStates> _searchState;
        private StatesBase<EnemyStates> _desperateSearchState;
        private StatesBase<EnemyStates> _attackState;

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
            var attackState = new MinotaurStateAttack<EnemyStates>(_pursuitSteering, weapon, attackManager,this);
            var chaseState = new MinotaurStateChase<EnemyStates>(_pursuitSteering, this);
            var searchState = new MinotaurStateSearch<EnemyStates>(_pursuitSteering);
            var desperateSearchState = new MinotaurStateDesperateSearch<EnemyStates>(_pursuitSteering, searchSpeed);


            _chaseState = chaseState;
            _searchState = searchState;
            _desperateSearchState = desperateSearchState;
            _attackState = attackState;

            var stateList = new List<State<EnemyStates>>
            {
                chaseState,
                searchState,
                desperateSearchState,
                attackState
            };
            
            chaseState.AddTransition(EnemyStates.Patrol, searchState);
            chaseState.AddTransition(EnemyStates.Surround, desperateSearchState);
            chaseState.AddTransition(EnemyStates.Attack, attackState);
            
            searchState.AddTransition(EnemyStates.Chase, chaseState);
            searchState.AddTransition(EnemyStates.Surround, desperateSearchState);
            searchState.AddTransition(EnemyStates.Attack, attackState);

            desperateSearchState.AddTransition(EnemyStates.Chase, chaseState);
            desperateSearchState.AddTransition(EnemyStates.Patrol, searchState);
            desperateSearchState.AddTransition(EnemyStates.Attack, attackState);
            
            attackState.AddTransition(EnemyStates.Chase,chaseState);
            attackState.AddTransition(EnemyStates.Attack, attackState);
            attackState.AddTransition(EnemyStates.Surround,desperateSearchState);
            attackState.AddTransition(EnemyStates.Patrol,searchState);

            InitializeComponents(stateList);
            Fsm.SetInit(chaseState, EnemyStates.Chase);
        }

        private void Die()
        {
            //Destroy(gameObject);
            SceneChanger.Instance.ChangeScene(sceneToChangeWhenDie);
        }
    }
}
