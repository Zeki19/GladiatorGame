using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "QuestionNodeSO", menuName = "Scriptable Objects/Tree Nodes/QuestionNode")]
public class QuestionNodeSO : TreeNodeSO
{
    [SerializeField] public QuestionEnum _question;
    [SerializeField] private TreeNodeSO _tNode;
    [SerializeField] private TreeNodeSO _fNode;
    private Dictionary<QuestionEnum, Func<AIContext, bool>> _questionFunc;

    private void OnEnable()
    {
        _questionFunc = new Dictionary<QuestionEnum, Func<AIContext, bool>>
        {
            { QuestionEnum.PlayerInAttackRange, PlayerInAttackRange },
            { QuestionEnum.PlayerIsInAStraightLine, PlayerIsInAStraightLine }
        };
    }
    public override void Execute(AIContext context)
    {
        if (_questionFunc == null)
            OnEnable();

        if (_questionFunc[_question](context)) 
        {
            _tNode.Execute(context);
        }
        else 
        {
            _fNode.Execute(context);
        }
    }
    private bool PlayerInAttackRange(AIContext context)
    {
        float distance = Vector3.Distance(context.SelfTransform.position, context.PlayerTransform.position);
        return distance <= context.AttackRange;
    }
    private bool PlayerIsInAStraightLine(AIContext context)
    {
        Vector3 origin = context.SelfTransform.position;
        Vector3 direction = (context.PlayerTransform.position - origin).normalized;
        float distance = Vector3.Distance(origin, context.PlayerTransform.position);

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
        {
            return hit.transform == context.PlayerTransform;
        }

        return false;
    }
}
