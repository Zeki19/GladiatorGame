using Entities;
using UnityEngine;

public abstract class EnemyController : EntityController
{
    [SerializeField] protected Rigidbody2D target;
    [SerializeField] protected float attackRange;

    protected ITreeNode Root;
    protected StateEnum CurrentState;
    protected virtual void Start()
    {
        InitializeFsm();
        InitializeTree();
    }
    
    protected override void Update()
    {
        base.Update();
        Root.Execute();
    }
    
    protected abstract void InitializeTree();
}
