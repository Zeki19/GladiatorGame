using UnityEngine;
using System;

[CreateAssetMenu(fileName = "QuestionNodeSO", menuName = "Scriptable Objects/Tree Nodes/QuestionNode")]
public class QuestionNodeSO : TreeNodeSO
{
    [SerializeField] public QuestionBase _question;
    [SerializeField] private TreeNodeSO _tNode;
    [SerializeField] private TreeNodeSO _fNode;
    public override void Execute()
    {
        if (_question.Execute()) 
        {
            _tNode.Execute();
        }
        else 
        {
            _fNode.Execute();
        }
    }
}
