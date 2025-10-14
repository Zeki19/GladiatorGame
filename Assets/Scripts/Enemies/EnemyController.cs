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
    public abstract class EnemyController : EntityController, IStatesData, ICondition, INavigation, IPausable
    {
        [SerializeField] protected Rigidbody2D target;
        [SerializeField] private NavMeshAgent NVagent;
        [SerializeField] protected string sceneToChangeWhenDie;
        [SerializeField] protected BehaviorGraphAgent _agent;
        protected StateDataManager stateDataManager = new StateDataManager();
        public int currentAttack;
        protected int _currentPhase = 1;
        public MissAttackHandler MissAttack=new MissAttackHandler();
        private bool _isGamePaused;

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
            PauseManager.OnCinematicStateChanged += HandlePause;
        }

        private void OnDestroy()
        {
            PauseManager.OnCinematicStateChanged -= HandlePause;
        }

        protected virtual void Start()
        {
            
        }

        protected virtual void Update()
        {
            Fsm.OnExecute();
            switch (manager.PhaseSystem.CurrentPhase())
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
            var arenaPainter = ServiceLocator.Instance.GetService<ArenaPainter>();
            arenaPainter?.PaintArena(transform, "Blood");
            _currentPhase = manager.PhaseSystem.CurrentPhase();
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
        private void HandlePause(bool paused)
        {
            _isGamePaused = paused;
            if (paused)
                OnPause();
            else
                OnResume();
        }
        public void OnPause()
        {
            enabled = false;
            _agent.enabled = false;
            _NVagent.ResetPath();
            _NVagent.velocity = Vector3.zero;
        }

        public void OnResume()
        {
            enabled = true;
            _agent.enabled = true;
        }

        #region Navmesh

        public void TurnOffNavMesh()
        {
            NVagent.updatePosition = false;
            NVagent.updateRotation = false;
        }

        public void RepositionInNavMesh()
        {
            StartCoroutine(ReturnToNavMesh());
        }

        private IEnumerator ReturnToNavMesh()
        {
            // Wait until the surface finishes building (optional delay)
            yield return new WaitForSeconds(0.2f);

            if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 3f, NavMesh.AllAreas))
            {
                Vector3 targetPos = hit.position;

                // Smooth slide back to NavMesh
                while (Vector3.Distance(transform.position, targetPos) > 0.05f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPos, 5f * Time.deltaTime);
                    yield return null;
                }

                // Sync agent
                NVagent.enabled = true;
                NVagent.Warp(transform.position);
                NVagent.updatePosition = true;
                NVagent.updateRotation = true;
            }
            else
            {
                Debug.LogWarning("Could not find NavMesh nearby after dash!");
            }
        }

        #endregion
    }
}