using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "zeki capo", story: "saltar [x] metros en [y] segundos", category: "Action", id: "4c923e7796059642f9e44d4c7f768408")]
public partial class ZekiCapoAction : Action
{
    [SerializeReference] public BlackboardVariable<float> X;
    [SerializeReference] public BlackboardVariable<float> Y;

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

