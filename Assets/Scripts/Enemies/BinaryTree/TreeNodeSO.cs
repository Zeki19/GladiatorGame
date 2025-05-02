using UnityEngine;

public abstract class TreeNodeSO : ScriptableObject, ITreeNode
{
    public abstract void Execute();
}
