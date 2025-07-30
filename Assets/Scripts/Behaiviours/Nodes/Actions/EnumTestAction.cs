using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Enum Test", story: "Testing [state]", category: "Action", id: "958def381d69fc18b1cb867fed3c074a")]
public abstract class EnumTestAction<T> : Action where T : Enum
{
    [SerializeReference] public BlackboardVariable<T> State;
    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected void Test(T a) => Debug.Log(a);

    protected override void OnEnd()
    {
    }
}