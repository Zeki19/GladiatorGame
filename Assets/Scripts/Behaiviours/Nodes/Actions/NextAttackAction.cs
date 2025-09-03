using System;
using Enemies;
using Enemies.Gaius;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "NextAttack", story: "The next attack is [number]", category: "Action", id: "b1cf09400caa25a414e5d1b901669909")]
public partial class NextAttackAction : Action
{
    [SerializeReference] public BlackboardVariable<int> Number;
    [SerializeReference] public BlackboardVariable<EnemyController> Controller;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Controller.Value.currentAttack = Number;
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

