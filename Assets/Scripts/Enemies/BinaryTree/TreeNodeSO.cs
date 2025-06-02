using Enemies;
using UnityEngine;

public abstract class TreeNodeSO : ScriptableObject, ITreeNode
{
    public abstract void Execute(AIContext context);
}
