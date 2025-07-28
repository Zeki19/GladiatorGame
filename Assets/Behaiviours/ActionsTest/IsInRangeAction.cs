using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "IsInRange", story: "is [Target] in [Range]", category: "Action", id: "5bfab6eda41e66b22ed70bd41c4bbaa1")]
public partial class IsInRangeAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<float> Range;
    

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if(Vector2.Distance(Self.Value.transform.position,Target.Value.transform.position)<Range)
            return Status.Success;
        else
        {
            return Status.Failure;
        }
    }

    protected override void OnEnd()
    {
    }
}

