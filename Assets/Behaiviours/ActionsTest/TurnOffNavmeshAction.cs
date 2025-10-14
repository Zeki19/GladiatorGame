using System;
using Enemies;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "TurnOffNavmesh", story: "Turn Off Navmesh", category: "Action", id: "aadbb6ffa17b32d0258b993d2f06d2f3")]
public partial class TurnOffNavmeshAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyController> Controller;

    protected override Status OnStart()
    {
        Controller.Value.TurnOffNavMesh();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnDeserialize()
    {
        base.OnDeserialize();
        Debug.Log("Deserialize");
    }

    protected override void OnEnd()
    {
    }
}

