using System;
using Entities;
using UnityEngine;

public abstract class boss1Controller : EntityController
{
    [SerializeField] protected Rigidbody2D target;

    protected ITreeNode Root;
    protected StateEnum CurrentState;

    protected override void Update()
    {
        base.Update();

        Root.Execute();
    }
    protected virtual void Start()
    {
        InitializeFsm();
        InitializeTree();
    }

    protected abstract void InitializeTree();
}
