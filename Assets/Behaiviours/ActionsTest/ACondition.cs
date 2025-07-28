using Enemies;
using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "a", story: "[aaas]", category: "Conditions", id: "85f8f95efa2496bb1e7b4af32b192df2")]
public partial class ACondition : Condition
{
    [SerializeReference] public BlackboardVariable<EnemyController> Aaas;

    public override bool IsTrue()
    {
        return true;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
