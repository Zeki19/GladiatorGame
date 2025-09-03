using System;
using Enemies;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "IsInCurrentState", story: "Is this the current state [state]", category: "Conditions", id: "92c438eebb5f507764c314fc845586b5")]
public partial class IsInCurrentStateCondition : Condition
{
    [SerializeReference] public BlackboardVariable<EnemyStates> State;
    [SerializeReference] public BlackboardVariable<EnemyController> Controller;
    private bool SetUp = false;

    public override bool IsTrue()
    {
        return Controller.Value.GetState()==State;
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
