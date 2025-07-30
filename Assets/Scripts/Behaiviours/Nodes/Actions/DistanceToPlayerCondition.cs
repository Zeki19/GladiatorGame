using System;
using Enemies;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Distance To Player", story: "Distance to player is [condicion] [value]", category: "Variable Conditions", id: "34fdfc35ca66a9afffeb27fbe12a67ce")]
public partial class DistanceToPlayerCondition : Condition
{
    [Comparison(comparisonType: ComparisonType.All)]
    [SerializeReference] public BlackboardVariable<ConditionOperator> Condicion;
    [SerializeReference] public BlackboardVariable<float> Value;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    private bool SetUp = false;
    

    public override bool IsTrue()
    {
        if (Value == null)
        {
            return false;
        }
        var distance = Vector2.Distance(Target.Value.transform.position, Self.Value.transform.position);
        return ConditionUtils.Evaluate(distance, Condicion, Value);
    }

    public override void OnStart()
    {
        if (!SetUp)
        {
            GameObject.GetComponent<BehaviorGraphAgent>().GetVariable("Target", out Target);
            GameObject.GetComponent<BehaviorGraphAgent>().GetVariable("Self", out Self);
            SetUp = true;
        }
    }

    public override void OnEnd()
    {
    }
}
