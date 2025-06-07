using System;
using Entities;
using UnityEngine;

namespace Enemies
{
    public abstract class EnemyController : EntityController
    {
        [SerializeField] protected Rigidbody2D target;
        [SerializeField] protected float attackRange;
        [SerializeField] protected TreeNodeSO Root;

        protected StateEnum CurrentState;
        protected AIContext objectContext;

        protected virtual void Awake()
        {
            InitializeFsm();
            objectContext = new AIContext
            {
                selfGameObject = gameObject,
                playerGameObject = target.gameObject,
                attackRange = attackRange,
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
        }

        protected override void Update()
        {
            base.Update();
            Root.Execute(objectContext);
        }
    
        protected abstract void InitializeTree();
    }
}
