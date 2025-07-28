using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CheckBool", story: "Check if [this] is [bool]", category: "Action", id: "b4efa42c42d65b403fb5235e01413ebf")]
public partial class CheckBoolAction : Action
{
    [SerializeReference] public BlackboardVariable<NewEnumForStatus> This;
    [SerializeReference] public BlackboardVariable<bool> Bool;
    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Bool.Value)
            return Status.Success;
        return Status.Failure;
    }

    protected override void OnEnd()
    {
    }
}

