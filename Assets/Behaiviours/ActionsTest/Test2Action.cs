using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "test2", story: "from this [so] is [sectered] is [bool]", category: "Action", id: "608ace01ca08da6d5f8b39f4ee86a65d")]
public partial class Test2Action : Action
{
    [SerializeReference] public BlackboardVariable<ScriptableObject> So;
    [SerializeReference] public BlackboardVariable<ListaDebools> Sectered;
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

