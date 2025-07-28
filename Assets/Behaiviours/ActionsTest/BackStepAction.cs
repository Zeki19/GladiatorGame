using System;
using Enemies;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "BackStep", story: "[Target] Backsteps [x] meters", category: "Action", id: "0d28c0d7b51c9b1c10fedba741f50b08")]
public partial class BackStepAction: Action
{
    [SerializeReference] public BlackboardVariable<EnemyModel> Target;
    [SerializeReference] public BlackboardVariable<EnemyController> Controller;
    [SerializeReference] public BlackboardVariable<float> X;

    protected override Status OnStart()
    {
        var dashData = new DashStateData(
            -GameObject.transform.up,
            10f,
            X.Value
        );
        Controller.Value.SetStateData(EnemyStates.Dash, dashData);
        Controller.Value.ChangeToState(EnemyStates.Dash);
        return Status.Running;

    }

    protected override Status OnUpdate()
    {
        if(!Controller.Value.GetStatus(StatusEnum.Dashing))
            return Status.Success;
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

