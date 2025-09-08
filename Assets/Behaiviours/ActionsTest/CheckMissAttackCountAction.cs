using System;
using Enemies;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CheckMissAttackCount", story: "Did i miss [X] attacks", category: "Action", id: "7e25f20b261359b064783869c47d45a7")]
public partial class CheckMissAttackCountAction : Action
{
    [SerializeReference] public BlackboardVariable<int> X;
    [SerializeReference] public BlackboardVariable<EnemyController> controller;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (controller.Value.MissAttack.CurrentMissAttacksCount >= X)
        {
            controller.Value.MissAttack.ResetMissAttacks();
            return Status.Success;
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

