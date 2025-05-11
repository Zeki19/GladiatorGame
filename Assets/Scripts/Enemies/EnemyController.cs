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
            stateMachine = Fsm
        };
        InitializeTree();
    }
    
    protected override void Update()
    {
        base.Update();
        Root.Execute(objectContext);
    }
    
    protected abstract void InitializeTree();
}
