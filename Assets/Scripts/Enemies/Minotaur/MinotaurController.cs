using System.Collections.Generic;
using Attack;
using Enemies.Valeria.States;
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

            var chaseState = new States.MinotaurStateChase<EnemyStates>(_pursuitSteering, this);
            var searchState = new States.MinotaurStateSearch<EnemyStates>(_pursuitSteering);
            var desperateSearchState = new States.MinotaurStateDesperateSearch<EnemyStates>(_pursuitSteering, searchSpeed);


            _chaseState = chaseState;
            _searchState = searchState;
            _desperateSearchState = desperateSearchState;

            var stateList = new List<State<EnemyStates>>
            {
                chaseState,
                searchState,
                desperateSearchState
            };
            
            chaseState.AddTransition(EnemyStates.RunAway, searchState);
            chaseState.AddTransition(EnemyStates.Surround, desperateSearchState);
            
            searchState.AddTransition(EnemyStates.Chase, chaseState);
            searchState.AddTransition(EnemyStates.Surround, desperateSearchState);

            desperateSearchState.AddTransition(EnemyStates.Chase, chaseState);
            desperateSearchState.AddTransition(EnemyStates.RunAway, searchState);

            InitializeComponents(stateList);
            Fsm.SetInit(chaseState, EnemyStates.Chase);
        }

        private void Die()
        {
            //Destroy(gameObject);
            SceneChanger.Instance.ChangeScene(sceneToChangeWhenDie);
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
