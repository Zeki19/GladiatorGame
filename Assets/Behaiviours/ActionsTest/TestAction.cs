using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Test", story: "aaaa", category: "Action", id: "c56626067d9a2f436ac34b4ac3e78c03")]
public partial class TestAction : Action
{

    public BlackboardVariable<float> Test;
    public BlackboardReference Reference;
    protected override Status OnStart()
    {
        Debug.Log(Reference);
        Debug.Log(Test);
        return Status.Running;
        
    }

    protected override Status OnUpdate()
    {
        Debug.Log(Test);
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

