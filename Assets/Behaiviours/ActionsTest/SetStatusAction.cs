using System;
using Entities;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetStatus", story: "Set [status] to [bool]", category: "Action", id: "ab2185c6a6ada99f24cb7e0441e23305")]
public partial class SetStatusAction : Action
{
    [SerializeReference] public BlackboardVariable<StatusEnum> status;
    [SerializeReference] public BlackboardVariable<bool> Bool;
    [SerializeReference] public BlackboardVariable<EntityController> Controller;
        
    private bool SetUp = false;

    protected override Status OnStart()
    {
        //if (!SetUp)
        //{
        //    GameObject.GetComponent<BehaviorGraphAgent>().GetVariable("Controller", out Controller);
        //    SetUp = true;
        //}
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Controller.Value.SetStatus(status, Bool.Value);
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

