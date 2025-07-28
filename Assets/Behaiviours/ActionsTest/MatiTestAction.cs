using Player;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MatiTest", story: "Is the [player] in [range]", category: "Action", id: "6390d9caab5eac074f6abc434ca0512d")]
public partial class MatiTestAction : Action
{
    [SerializeReference] public BlackboardVariable<PlayerManager> Player;
    [SerializeReference] public BlackboardVariable<int> Range;

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

