using System;
using Enemies;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AttackFinished", story: "Has the attack finish?", category: "Action", id: "a91d1dc138377c6225a64eb10a023a40")]
public partial class AttackFinishedAction : Action
{

    [SerializeReference] public BlackboardVariable<EnemyController> Controller;
    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Controller.Value.GetStatus(StatusEnum.Attacking))
            return Status.Running;
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

