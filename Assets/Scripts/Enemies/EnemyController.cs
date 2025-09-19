using System;
using System.Collections;
using Entities;
using System.Collections.Generic;
using Entities.StateMachine;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Enemies
{
    public abstract class EnemyController : EntityController, IStatesData, ICondition, INavigation
    {
        [SerializeField] protected Rigidbody2D target;
        [SerializeField] private NavMeshAgent NVagent;
        [SerializeField] protected string sceneToChangeWhenDie;
        [SerializeField] protected BehaviorGraphAgent _agent;
        protected StateDataManager stateDataManager = new StateDataManager();
        public int currentAttack;
        protected int _currentPhase = 1;
        public MissAttackHandler MissAttack=new MissAttackHandler();

        public int CurrentPhase
        {
            get { return _currentPhase; }
        }
        public NavMeshAgent _NVagent => NVagent;
        
        protected FSM<EnemyStates> Fsm;

        protected override void Awake()
        {
            base.Awake();
            InitializeFsm();
        }

        protected virtual void Start()
        {
            
        }

        protected virtual void Update()
        {
            Fsm.OnExecute();
            switch (manager.PhaseSystem.currentPhase())
            {
                case 1:_agent.SetVariableValue("CurrentPhase",global::CurrentPhase.Phase1);
                    break;
                case 2:_agent.SetVariableValue("CurrentPhase",global::CurrentPhase.Phase2);
                    break;
                case 3:_agent.SetVariableValue("CurrentPhase",global::CurrentPhase.Phase3);
                    break;
            }
        }

        protected void CheckPhase(float damage)
        {
            manager.PlaySound("Hit","Enemy");
            ServiceLocator.Instance.GetService<ArenaPainter>().PaintArena(transform, "Blood");
            _currentPhase = manager.PhaseSystem.currentPhase();
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