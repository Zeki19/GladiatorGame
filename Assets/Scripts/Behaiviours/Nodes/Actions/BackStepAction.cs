using System;
using Enemies;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.Serialization;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "BackStep", story: "[Target] Backsteps [Distance] meters", category: "Action", id: "0d28c0d7b51c9b1c10fedba741f50b08")]
public partial class BackStepAction: Action
{
    public bool setUp = false;
    private BlackboardVariable<EnemyController> controller;
    [SerializeReference] public BlackboardVariable<float> Distance;
    [SerializeReference] public BlackboardVariable<float> Force;

    protected override Status OnStart()
    {
        if (!setUp)
        {
            GameObject.GetComponent<BehaviorGraphAgent>().GetVariable("Controller", out controller);
            setUp = true;
        }
        var dashData = new DashStateData(
            -GameObject.transform.up,
            Force.Value,
            Distance.Value
        );
        controller.Value.SetStateData(EnemyStates.Dash, dashData);
        controller.Value.ChangeToState(EnemyStates.Dash);
        return Status.Running;

    }

    protected override Status OnUpdate()
    {
        if(!controller.Value.GetStatus(StatusEnum.Dashing))
            return Status.Success;
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

