using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "as", story: "[a] is [bool]", category: "Action", id: "028de0576f3c5047bbd69e9d8db9c595")]
public partial class AsAction : Action
{
    [SerializeReference] public BlackboardVariable<Status> A;
    [SerializeReference] public BlackboardVariable<bool> Bool;

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

