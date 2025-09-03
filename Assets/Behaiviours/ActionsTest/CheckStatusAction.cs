using System;
using Enemies;
using Entities;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Behaiviours.ActionsTest
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "CheckStatus", story: "is [status] True", category: "Action", id: "f5e530d9ebfd777179adf1fcd7a9f0d2")]
    public partial class CheckStatusAction : Action
    {
        [SerializeReference] public BlackboardVariable<StatusEnum> status;
        [SerializeReference] public BlackboardVariable<EnemyController> Controller;
        
        private bool SetUp = false;

        protected override Status OnStart()
        {
            if (!SetUp)
            {
                GameObject.GetComponent<BehaviorGraphAgent>().GetVariable("Controller", out Controller);
                SetUp = true;
            }
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
        if(Controller.Value.GetStatus(status))
                return Status.Success;
            else
                return Status.Failure;
        }

        protected override void OnEnd()
        {
        }
    }
}

