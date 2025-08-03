using System;
using System.Collections.Generic;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

namespace Behaiviours.ActionsTest
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "WeightedRandomNode ", story: "[weights]", category: "Flow", id: "09733e6dcf59be9787c379392357bf6d")]
    internal partial class WeightedRandomNode : Composite
    {
        private struct WeightedChoice
        {
            public int ChildIndex;
            public float Weight;
        }

        [SerializeField] private BlackboardVariable<List<float>> weights;
        private WeightedChoice[] choices;
        private float totalWeight;

        protected override Status OnStart()
        {
            choices = new WeightedChoice[Children.Count];
            totalWeight = 0f;

            for (int i = 0; i < Children.Count; i++)
            {
                if (i < weights.Value.Count)
                {
                    choices[i] = new WeightedChoice 
                    { 
                        ChildIndex = i, 
                        Weight = weights.Value[i] 
                    };
                    totalWeight += weights.Value[i];
                }
            }

            return Status.Running;
        }
    
        protected override Status OnUpdate()
        {
            // Select a random child based on weights
            float random = UnityEngine.Random.Range(0f, totalWeight);
            float cumulative = 0f;

            for (int i = 0; i < choices.Length; i++)
            {
                cumulative += choices[i].Weight;
                if (random <= cumulative)
                {
                    return Children[choices[i].ChildIndex].CurrentStatus;
                }
            }

            // Fallback to last child if something went wrong
            return Children[Children.Count - 1].CurrentStatus;
        }
    }
}

