using UnityEngine;

public abstract class TreeNodeSO : ScriptableObject
{
    public abstract void Execute(AIContext context);
}
