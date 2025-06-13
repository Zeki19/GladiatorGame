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

        private States_Base<StateEnum> _idleState;
        private States_Base<StateEnum> _attackState;

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
            var attackState = new GaiusStateAttack<StateEnum>(SpriteRendererBoss);

            _idleState = idleState;
            _attackState = attackState;

            var stateList = new List<States_Base<StateEnum>>
            {
                idleState,
                attackState,
            };
            idleState.AddTransition(StateEnum.Attack, attackState);

            attackState.AddTransition(StateEnum.Idle, idleState);

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