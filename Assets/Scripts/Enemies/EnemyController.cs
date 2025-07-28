using System;
using System.Collections;
using Entities;
using System.Collections.Generic;
using Entities.StateMachine;
using UnityEngine;

namespace Enemies
{
    public abstract class EnemyController : EntityController, IStatus ,IStatesData,ICondition
    {
        [SerializeField] protected Rigidbody2D target;
        [SerializeField] protected int[] phasesThresholds;
        [SerializeField] private Dictionary<StatusEnum, bool> Status=new Dictionary<StatusEnum, bool>();
        protected StateDataManager stateDataManager = new StateDataManager();
        
        protected int _currentPhase = 1;

        public int CurrentPhase
        {
            get { return _currentPhase; }
        }

        protected List<float> attackRanges = new List<float>();
        protected PhaseSystem _phaseSystem;
        protected FSM<EnemyStates> Fsm;

        protected virtual void Awake()
        {
            InitializeFsm();
            foreach (StatusEnum status in (StatusEnum[]) Enum.GetValues(typeof(StatusEnum)))
            {
                Status.Add(status, false);
            }
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
            manager.PlaySound("Hit", "Enemy");
            Debug.Log("Current phase is:" + _currentPhase);
        }

        public bool GetStatus(StatusEnum status)
        {
            return Status[status];
        }

        public void SetStatus(StatusEnum status, bool value)
        {
            Status[status] = value;
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
        
        public void StartDashMonitoring(Vector2 dir, float distance, Vector2 startingPosition)
        {
            StartCoroutine(MonitorDashDistance(dir, distance, startingPosition));
        }

        public IEnumerator MonitorDashDistance(Vector2 dir, float distance,Vector2 startingPosition)
        {
            Vector2 targetPosition = startingPosition + dir * distance;
            float targetDistance = Vector2.Distance(startingPosition, targetPosition);
            while (true)
            {
                float currentDistance = Vector2.Distance(startingPosition, (Vector2)transform.position);
                if (currentDistance >= targetDistance)
                {
                    break;
                }
                yield return null;
            }
            manager.Rb.linearVelocity = Vector2.zero;
            SetStatus(StatusEnum.Dashing,false);
        }
    }
}