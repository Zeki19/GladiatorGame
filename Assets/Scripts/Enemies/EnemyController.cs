using System.Collections.Generic;
using Enemies;
using Entities;
using UnityEngine;

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
            controller = this
        };
        objectContext.Points.Add((new Vector2(0, 0), 5));
        objectContext.Points.Add((new Vector2(0, 0), 8));
        InitializeTree();
    }
    
    protected override void Update()
    {
        base.Update();
        Root.Execute(objectContext);
    }
    
    protected abstract void InitializeTree();
}
