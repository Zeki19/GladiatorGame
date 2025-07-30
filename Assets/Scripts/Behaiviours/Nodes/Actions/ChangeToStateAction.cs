using System;
using Enemies;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Change To State", story: "Change to State [state]", category: "Action", id: "4444b43794cb3059a1478a52b68c5112")]
public partial class ChangeToStateAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyStates> State;
    [SerializeReference] public BlackboardVariable<EnemyController> Controller;

    protected override Status OnStart()
    {
        Controller.Value.ChangeToState(State.Value);
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if(!Controller.Value.GetStatus(StatusEnum.Attacking))
            return Status.Success;
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

