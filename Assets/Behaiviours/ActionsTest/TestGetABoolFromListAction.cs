using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "TestGetABoolFromList", story: "Test if i can get [this] bool", category: "Action", id: "a28a6e7f0e75988d7db87b5d8c891d26")]
public partial class TestGetABoolFromListAction : Action
{
    [SerializeReference] public BlackboardVariable<List<bool>> This;

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
}

