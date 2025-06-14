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

        private States_Base<StateEnum> _idleState; // BLUE
        private States_Base<StateEnum> _stunState; // GREEN
        private States_Base<StateEnum> _chaseState; // WHITE
        private States_Base<StateEnum> _attackState; // NONAPLICABLE
        private States_Base<StateEnum> _shortAttackState; // YELLOW
        private States_Base<StateEnum> _midAttackState; // RED
        private States_Base<StateEnum> _longAttackState; // BLACK

        #endregion

        private PhaseSystem _phaseSystem;
        private int _currentPhase = 1;
        
        protected override void Awake()
        {
            base.Awake();
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

            var idleState = new GaiusStateIdle<StateEnum>( SpriteRendererBoss);
            var stunState = new GaiusStateStun<StateEnum>(SpriteRendererBoss);
            var chaseState = new GaiusStateChase<StateEnum>(SpriteRendererBoss);
            var attackState = new GaiusStateAttack<StateEnum>(SpriteRendererBoss);
            var shortAttackState = new GaiusStateShortAttack<StateEnum>(SpriteRendererBoss);
            var midAttackState = new GaiusStateMidAttack<StateEnum>(SpriteRendererBoss);
            var longAttackState = new GaiusStateLongAttack<StateEnum>(SpriteRendererBoss);

            _idleState = idleState;
            _stunState = stunState;
            _chaseState = chaseState;
            _attackState = attackState;
            _shortAttackState = shortAttackState;
            _midAttackState = midAttackState;
            _longAttackState = longAttackState;


            var stateList = new List<States_Base<StateEnum>>
            {
                idleState,
                stunState,
                chaseState,
                attackState,
                shortAttackState,
                midAttackState,
                longAttackState
            };

            idleState.AddTransition(StateEnum.Chase, chaseState);
            idleState.AddTransition(StateEnum.ShortAttack, shortAttackState);
            idleState.AddTransition(StateEnum.MidAttack, midAttackState);
            idleState.AddTransition(StateEnum.LongAttack, longAttackState);

            stunState.AddTransition(StateEnum.Idle, idleState);
            stunState.AddTransition(StateEnum.Chase, chaseState);

            chaseState.AddTransition(StateEnum.Idle, idleState);
            chaseState.AddTransition(StateEnum.Stun, stunState);
            chaseState.AddTransition(StateEnum.ShortAttack, shortAttackState);
            chaseState.AddTransition(StateEnum.MidAttack, midAttackState);
            chaseState.AddTransition(StateEnum.LongAttack, longAttackState);

            attackState.AddTransition(StateEnum.Idle, idleState);
            
            shortAttackState.AddTransition(StateEnum.Idle, idleState);
            
            midAttackState.AddTransition(StateEnum.Idle, idleState);
            
            longAttackState.AddTransition(StateEnum.Idle, idleState);
            longAttackState.AddTransition(StateEnum.Stun, stunState);


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