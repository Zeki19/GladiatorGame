using UnityEngine;

namespace Enemies.BinaryTree
{
    [CreateAssetMenu(fileName = "ActionNodeSO", menuName = "Scriptable Objects/Tree Nodes/ActionNode")]
    public class ActionNodeSO : TreeNodeSO
    {
        [SerializeField] public StateEnum stateEnum;
        public override void Execute(AIContext context)
        {
            context.stateMachine.Transition(stateEnum);
        }
    }
}
