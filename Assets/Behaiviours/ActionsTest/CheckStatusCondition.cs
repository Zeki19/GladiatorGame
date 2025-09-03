using System;
using Enemies;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CheckStatus", story: "is [status] True", category: "Conditions", id: "0b4717688f31040c9c8d22b6d7808955")]
public partial class CheckStatusCondition : Condition
{
    [SerializeReference] public BlackboardVariable<StatusEnum> status;
    [SerializeReference] public BlackboardVariable<EnemyController> Controller;
    private bool SetUp = false;

    public override bool IsTrue()
    {
        return Controller.Value.GetStatus(status);
    }

    public override void OnStart()
    {
        if (!SetUp)
        {
            GameObject.GetComponent<BehaviorGraphAgent>().GetVariable("Controller", out Controller);
            SetUp = true;
        }
    }

    public override void OnEnd()
    {
    }
}
