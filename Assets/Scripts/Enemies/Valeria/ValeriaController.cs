using System.Collections.Generic;
using Attack;
using Enemies.Valeria.States;
using Entities.StateMachine;
using Unity.Behavior;
using UnityEngine;

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
        [SerializeField] private float desiredMeleeDistance;
        
        #endregion

        [SerializeField] private GameObject weapon;
        [SerializeField] private AttackManager attackManager;
        [SerializeField] private LayerMask hiddingLayer;
        [SerializeField] private float hiddingTime;
        [SerializeField] private float invisibilitySpeed;
        [SerializeField] private GameObject stepPrefab;
        [SerializeField] private float shortMeleeAttackCd;
        [SerializeField] private float longMeleeAttackCd;
        [SerializeField] private float longAttackCd;
        [SerializeField] private float middleAttackCd;
        public BehaviorGraphAgent agent;
        #region Private Variables

        private StatesBase<EnemyStates> _runAwayState;
        private StatesBase<EnemyStates> _chaseState;
        private StatesBase<EnemyStates> _invisibilityState;
        private StatesBase<EnemyStates> _dashState;
        private StatesBase<EnemyStates> _AttackState;
        private StatesBase<EnemyStates> _meleeState;
        private StatesBase<EnemyStates> _ambushState;
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
            manager.HealthComponent.OnDamage += CheckPhase;
            manager.HealthComponent.OnDead += Die;
            ServiceLocator.Instance.RegisterService<ValeriaController>(this);
        }

        void InitalizeSteering()
        {
            _pursuitSteering = new StPursuit(transform, target, 0);
        }

        protected override void InitializeFsm()
        {
            Fsm = new FSM<EnemyStates>();


            var dashState = new ValeriaStateDash<EnemyStates>(manager);
            var attackState = new ValeriaStateAttack<EnemyStates>(_pursuitSteering, weapon, attackManager,this, manager);
            var chaseState = new ValeriaStateChase<EnemyStates>(_pursuitSteering, target, desiredDistance, stoppingThreshold, orbitSpeed, orbitAngle, cooldown,longAttackCd,middleAttackCd);
            var meleeState = new ValeriaStateMeleeLock<EnemyStates>(_pursuitSteering, target, desiredMeleeDistance, stoppingThreshold, orbitSpeed, orbitAngle, cooldown,shortMeleeAttackCd,longMeleeAttackCd);
            var runAwayState = new ValeriaStateRunAway<EnemyStates>(target, hiddingLayer, hiddingTime);
            var invisibilityState = new ValeriaStateInvisibility<EnemyStates>(target, invisibilitySpeed, stepPrefab);
            var ambushState = new ValeriaStateAmbush<EnemyStates>(target, 20);
            var deathState = new DeadState<EnemyStates>();
            
            _dashState = dashState;
            _chaseState = chaseState;
            _meleeState = meleeState;
            _AttackState = attackState;
            _runAwayState = runAwayState;
            _invisibilityState = invisibilityState;
            _ambushState = ambushState;
            _deathState = deathState;

            var stateList = new List<State<EnemyStates>>
            {
                chaseState,
                dashState,
                attackState,
                runAwayState,
                invisibilityState,
                meleeState,
                ambushState,
                deathState
            };
            
            chaseState.AddTransition(EnemyStates.Attack, attackState);
            chaseState.AddTransition(EnemyStates.Dash, dashState);
            chaseState.AddTransition(EnemyStates.RunAway, runAwayState);
            chaseState.AddTransition(EnemyStates.Invisibility, invisibilityState);
            chaseState.AddTransition(EnemyStates.Surround, meleeState);
            chaseState.AddTransition(EnemyStates.Death, deathState);
            
            meleeState.AddTransition(EnemyStates.Surround, chaseState);
            meleeState.AddTransition(EnemyStates.Attack, attackState);
            meleeState.AddTransition(EnemyStates.Dash, dashState);
            meleeState.AddTransition(EnemyStates.RunAway, runAwayState);
            meleeState.AddTransition(EnemyStates.Invisibility, invisibilityState);
            meleeState.AddTransition(EnemyStates.Death, deathState);

            attackState.AddTransition(EnemyStates.Chase, chaseState);
            attackState.AddTransition(EnemyStates.Dash, dashState);
            attackState.AddTransition(EnemyStates.RunAway, runAwayState);
            attackState.AddTransition(EnemyStates.Invisibility, invisibilityState);
            attackState.AddTransition(EnemyStates.Surround, meleeState);
            attackState.AddTransition(EnemyStates.Death, deathState);
            
            dashState.AddTransition(EnemyStates.Chase, chaseState);
            dashState.AddTransition(EnemyStates.Attack, attackState);
            dashState.AddTransition(EnemyStates.RunAway, runAwayState);
            dashState.AddTransition(EnemyStates.Invisibility, invisibilityState);
            dashState.AddTransition(EnemyStates.Surround, meleeState);
            dashState.AddTransition(EnemyStates.Death, deathState);

            
            runAwayState.AddTransition(EnemyStates.Chase, chaseState);
            runAwayState.AddTransition(EnemyStates.Attack, attackState);
            runAwayState.AddTransition(EnemyStates.Dash, dashState);
            runAwayState.AddTransition(EnemyStates.Invisibility, invisibilityState);
            runAwayState.AddTransition(EnemyStates.Surround, meleeState);
            runAwayState.AddTransition(EnemyStates.Death, deathState);

            
            invisibilityState.AddTransition(EnemyStates.Chase, chaseState);
            invisibilityState.AddTransition(EnemyStates.Attack, attackState);
            invisibilityState.AddTransition(EnemyStates.RunAway, runAwayState);
            invisibilityState.AddTransition(EnemyStates.Dash, dashState);
            invisibilityState.AddTransition(EnemyStates.Surround, meleeState);
            invisibilityState.AddTransition(EnemyStates.Ambush, ambushState);
            invisibilityState.AddTransition(EnemyStates.Death, deathState);

            ambushState.AddTransition(EnemyStates.Chase, chaseState);
            ambushState.AddTransition(EnemyStates.Attack, attackState);
            ambushState.AddTransition(EnemyStates.RunAway, runAwayState);
            ambushState.AddTransition(EnemyStates.Dash, dashState);
            ambushState.AddTransition(EnemyStates.Surround, meleeState);
            ambushState.AddTransition(EnemyStates.Invisibility, invisibilityState);
            ambushState.AddTransition(EnemyStates.Death, deathState);

            InitializeComponents(stateList);
            Fsm.SetInit(chaseState, EnemyStates.Chase);
        }
    
        private void OnDrawGizmos()
        {
            if(Fsm == null)
            {
                return;
            }
            if (Fsm.CurrentState() == _runAwayState)
            {
                var runaAway = _runAwayState as ValeriaStateRunAway<EnemyStates>;
                Vector3 destination = runaAway.currentDestination;

                if (destination != Vector3.zero)
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawLine(transform.position, destination);
                    Gizmos.DrawSphere(destination, 0.2f);
                }
            }
        }
        public void KillEnemy()
        {
            Die();
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
        //private void OnDestroy()
        //{
        //    ServiceLocator.Instance.RemoveService<ValeriaController>(this);
        //}

    }
}
