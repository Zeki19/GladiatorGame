using System;
using Enemies;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AttackMissed", story: "Did the attack miss?", category: "Action", id: "7e1c812da906cb02d2f5c703ea2ad95f")]
public partial class AttackMissedAction : Action
{
    
    
    [SerializeReference] public BlackboardVariable<EnemyController> Controller;
    
    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Controller.Value.GetStatus(StatusEnum.AttackMissed))
            return Status.Success;
        return Status.Failure;
    }

    protected override void OnEnd()
    {
    }
}

