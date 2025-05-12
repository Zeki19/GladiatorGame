using System;
using UnityEngine;

public class ActionNode : ITreeNode
{
    private Action _action;

    public ActionNode(Action action)
    {
        _action = action;
    }
    public void Execute(AIContext context)
    {
        _action();
    }
}
