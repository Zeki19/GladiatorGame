using System;
using Entities;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public abstract class EnemyController : EntityController
    {
        [SerializeField] protected Rigidbody2D target;
        [SerializeField] protected TreeNodeSO Root;
        [SerializeField] protected int[] phasesThresholds;

        protected int _currentPhase = 1;
        public int CurrentPhase { get { return _currentPhase; } }
        protected List<float> attackRanges = new List<float>();
        protected PhaseSystem _phaseSystem;
        protected StateEnum CurrentState;
        protected AIContext objectContext;

        protected virtual void Awake()
        {
            InitializeFsm();
            objectContext = new AIContext
            {
                selfGameObject = gameObject,
                playerGameObject = target.gameObject,
                attackRanges = attackRanges,
                stateMachine = Fsm,
                controller = this,
                model=manager.model as EnemyModel
            };
            objectContext.Points.Add((new Vector2(0, 0), 5));
            objectContext.Points.Add((new Vector2(0, 0), 10));
        }

        protected virtual void Start()
        {
            InitializeTree();
            _phaseSystem = new PhaseSystem(phasesThresholds, manager.HealthComponent);
        }

        protected override void Update()
        {
            base.Update();
            Root.Execute(objectContext);
        }
        protected void CheckPhase(float damage)
        {
            manager.PlaySound("Hit","Enemy");
            ServiceLocator.Instance.GetService<ArenaPainter>().PaintArena(transform, "Blood");
            _currentPhase = _phaseSystem.currentPhase();
            Debug.Log("Current phase is:" + _currentPhase);
        }
        public StateEnum GetState() 
        {
            return Fsm.CurrentStateEnum();
        }
        protected abstract void InitializeTree();
    }
}
