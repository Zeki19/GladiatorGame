using System;
using Enemies;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ReturnToNavmesh", story: "Return to NavMesh", category: "Action", id: "aecabb1fc18487ebd205608cd343f328")]
public partial class ReturnToNavmeshAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyController> Controller;

    protected override Status OnStart()
    {
        Controller.Value.RepositionInNavMesh();
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

