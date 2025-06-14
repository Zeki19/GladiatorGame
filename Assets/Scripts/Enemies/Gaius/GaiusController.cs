using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [SerializeField] public float shortRange;

        [SerializeField] public float midRange;

        [SerializeField] public float longRange;

        public SpriteRenderer SpriteRendererBoss;

        public bool didAttackMiss;

        #region Private Variables

        private States_Base<StateEnum> _idleState; // BLUE
        private States_Base<StateEnum> _stunState; // GREEN
        private States_Base<StateEnum> _chaseState; // WHITE
        private States_Base<StateEnum> _shortAttackState; // YELLOW
        private States_Base<StateEnum> _midAttackState; // RED
        private States_Base<StateEnum> _longAttackState; // BLACK

        #endregion

        protected override void Awake()
        {
            attackRanges.Add(shortRange);
            attackRanges.Add(midRange);
            attackRanges.Add(longRange);
            base.Awake();
            SpriteRendererBoss = GetComponent<SpriteRenderer>();
        }

        protected override void Start()
        {
            base.Start();
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
            var shortAttackState = new GaiusStateShortAttack<StateEnum>(SpriteRendererBoss);
            var midAttackState = new GaiusStateMidAttack<StateEnum>(SpriteRendererBoss);
            var longAttackState = new GaiusStateLongAttack<StateEnum>(SpriteRendererBoss);

            _idleState = idleState;
            _stunState = stunState;
            _chaseState = chaseState;
            _shortAttackState = shortAttackState;
            _midAttackState = midAttackState;
            _longAttackState = longAttackState;


            var stateList = new List<States_Base<StateEnum>>
            {
                idleState,
                stunState,
                chaseState,
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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, shortRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, midRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, longRange);

        }

        private void Die()
        {
            Destroy(gameObject);
        }

        private void OnValidate()
        {
            shortRange = Mathf.Clamp(shortRange, 0, midRange);
            midRange = Mathf.Clamp(midRange, shortRange, longRange);
            longRange = Mathf.Max(longRange, midRange);
        }
    }
}