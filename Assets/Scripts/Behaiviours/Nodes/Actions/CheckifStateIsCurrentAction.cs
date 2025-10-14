using System;
using Enemies;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CheckifStateIsCurrent", story: "Is this the current state [a]", category: "Action", id: "df6f50cd5579113fc6a9a1f5f937603c")]
public partial class CheckifStateIsCurrentAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyStates> A;
    [SerializeReference] public BlackboardVariable<EnemyController> Controller;
    [SerializeReference] public BlackboardVariable<bool> CanNotFail;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if(Controller.Value.GetState()==A)
            return Status.Success;
        else
        {
            if(!CanNotFail)
                return Status.Failure;
            return Status.Running;
        }
    }

    protected override void OnEnd()
    {
    }

    protected override void OnSerialize()
    {
        base.OnSerialize();
    }
    
}

