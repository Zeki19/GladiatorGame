using System;
using System.Collections.Generic;
using Enemies;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;


[NodeDescription(name: "TestList", story: "yes [Controller] Controller", category: "Action",
    id: "dcefa7dd1fd5a3a2314702ae5aa2b966")]
public partial class TestListAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyController> Controller;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }

    protected override void AwakeParents()
    {
        base.AwakeParents();
        Debug.Log("a");
    }
}