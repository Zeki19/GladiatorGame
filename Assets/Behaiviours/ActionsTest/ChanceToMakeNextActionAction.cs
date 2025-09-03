using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Behaiviours.ActionsTest
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "TryToAttack", story: "The is an [Chance] % chance of making the next action", category: "Action", id: "0c16035748d0c59a6a24be8a392ebf67")]
    public partial class ChanceToMakeNextActionAction : Action
    {
        [SerializeReference] public BlackboardVariable<float> Chance;

        protected override Status OnStart()
        {
            var random = UnityEngine.Random.Range(0f, 100f);
            Debug.Log(random);
            if (random < Chance.Value)
                return Status.Success;
            else
                return Status.Failure;
        }

        

        protected override void OnEnd()
        {
        }
    }
}

