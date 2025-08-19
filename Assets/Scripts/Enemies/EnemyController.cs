using System;
using System.Collections;
using Entities;
using System.Collections.Generic;
using Entities.StateMachine;
using UnityEngine;

namespace Enemies
{
    public abstract class EnemyController : EntityController,IStatesData,ICondition
    {
        [SerializeField] protected Rigidbody2D target;
        [SerializeField] protected int[] phasesThresholds;
        protected StateDataManager stateDataManager = new StateDataManager();
        public int currentAttack;
        protected int _currentPhase = 1;

        public int CurrentPhase
        {
            get { return _currentPhase; }
        }
        protected PhaseSystem _phaseSystem;
        protected FSM<EnemyStates> Fsm;

        protected override void Awake()
        {
            base.Awake();
            InitializeFsm();
        }

        protected virtual void Start()
        {
            _phaseSystem = new PhaseSystem(phasesThresholds, manager.HealthComponent);
        }

        protected virtual void Update()
        {
            Fsm.OnExecute();
        }

        protected void CheckPhase(float damage)
        {

            manager.PlaySound("Hit","Enemy");
            ServiceLocator.Instance.GetService<ArenaPainter>().PaintArena(transform, "Blood");
            _currentPhase = _phaseSystem.currentPhase();
            Debug.Log("Current phase is:" + _currentPhase);
        }

        public EnemyStates GetState()
        {
            return Fsm.CurrentStateEnum();
        }

        public void ChangeToState(EnemyStates state)
        {
            Fsm.Transition(state);
        }
        public void SetStateData(EnemyStates state, IStateData data)
        {
            stateDataManager.SetStateData(state, data);
        }

        public T GetStateData<T>(EnemyStates state) where T : class, IStateData
        {
            return stateDataManager.GetStateData<T>(state);
        }
    }
}