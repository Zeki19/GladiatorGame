using System;
using Enemies;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "RepositionForThrowing", story: "Dash [x] units to find the [player]", category: "Action", id: "40810842e6bca16e211df74564d092c3")]
public partial class RepositionForThrowingAction : Action
{
    [SerializeReference] public BlackboardVariable<float> X;
    [SerializeReference] public BlackboardVariable<EnemyModel> model;
    [SerializeReference] public BlackboardVariable<GameObject> player;
    

    protected override Status OnStart()
    {
        model.Value.Reposition(player.Value.transform,X,LayerMask.GetMask("BreakableObject"));
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

