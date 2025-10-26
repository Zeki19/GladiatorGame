using System;
using Enemies;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CheckIfTargetIsInSight", story: "is [target] is sight", category: "Action", id: "b9296ebbee82937b32b3ae4340663749")]
public partial class CheckIfTargetIsInSightAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<EnemyModel> model;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        RaycastHit2D hit = model.Value.RaycastBetweenCharacters(GameObject.transform, Target.Value.transform,model.Value.obstaclesMask);
        if(hit.transform!=null)
            return Status.Running;
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

