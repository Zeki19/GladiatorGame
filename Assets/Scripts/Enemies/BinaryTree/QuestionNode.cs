using System;
using Enemies;
using UnityEngine;

public class QuestionNode : ITreeNode
{
    private Func<bool> _question;
    private ITreeNode _tNode;
    private ITreeNode _fNode;

    public QuestionNode(Func<bool> question, ITreeNode tNode, ITreeNode fNode)
    {
        _question = question;
        _tNode = tNode;
        _fNode = fNode;
    }
    public void Execute(AIContext context)
    {
        if (_question())
        {
            _tNode.Execute(context);
        }
        else
        {
            _fNode.Execute(context);
        }
    }
}
