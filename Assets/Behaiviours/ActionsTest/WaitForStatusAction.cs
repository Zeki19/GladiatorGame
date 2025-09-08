using System;
using Enemies;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "WaitForStatus", story: "waiting for [MyStatus] to be [x]", category: "Action", id: "2a0161c2a21119966f277e0de08950df")]
public partial class WaitForStatusAction : Action
{
    [SerializeReference] public BlackboardVariable<StatusEnum> MyStatus;
    [SerializeReference] public BlackboardVariable<bool> X;
    [SerializeReference] public BlackboardVariable<EnemyController> Controller;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Controller.Value.GetStatus(MyStatus)==X)
            return Status.Success;
        else
        {
            return Status.Running;
        }
    }

    protected override void OnEnd()
    {
    }
}

