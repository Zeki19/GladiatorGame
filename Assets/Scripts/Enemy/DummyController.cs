using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class DummyController : MonoBehaviour
{
    public Transform target;
    private FSM<StateEnum> _fsm;
    private DummyModel _model;
    private LineOfSight _los;
    private ITreeNode _root;

    private void Awake()
    {
        _model = GetComponent<DummyModel>();
        _los = GetComponent<LineOfSight>();
    }

    void Start()
    {
        InitializedFsm();
        InitializedTree();
    }

    void Update()
    {
        _fsm.OnExecute(Vector2.right);
        _root.Execute();
    }

    private void FixedUpdate()
    {
        _fsm.OnFixedExecute();
    }

    void InitializedFsm()
    {
        _fsm = new FSM<StateEnum>();
        
        var look = GetComponent<ILook>();
        
        var idle = new DSIdle<StateEnum>();
        var attack = new DSAttack<StateEnum>();
        var chase = new DSChase<StateEnum>(target, _model.speed);

        var stateList = new List<DSBase<StateEnum>>();
        stateList.Add(idle);
        stateList.Add(attack);
        stateList.Add(chase);
        
        idle.AddTransition(StateEnum.Chase, chase);
        idle.AddTransition(StateEnum.Attack, attack);
        
        attack.AddTransition(StateEnum.Chase, chase);
        attack.AddTransition(StateEnum.Idle, idle);
        
        chase.AddTransition(StateEnum.Attack, attack);
        chase.AddTransition(StateEnum.Idle, idle);

        foreach (var t in stateList)
        {
            t.Initialize(_model, look, _model);
        }
        
        _fsm.SetInit(idle);
    }

    void InitializedTree()
    {
        var idle = new ActionNode(()=>_fsm.Transition(StateEnum.Idle));
        var chase = new ActionNode(() => _fsm.Transition(StateEnum.Chase));
        var attack = new ActionNode(() => _fsm.Transition(StateEnum.Attack));

        var qCanAttack = new QuestionNode(QuestionCanAttack, attack, chase);
        var qTargetInView = new QuestionNode(QuestionTargetInView, qCanAttack, chase);

        _root = qTargetInView;
    }

    bool QuestionCanAttack()
    {
        return Vector2.Distance(_model.Position, target.position) <= _model.attackRange;
    }
    
    bool QuestionTargetInView()
    {
        if (target == null) return false;
        return _los.LOS(target.transform);
    }
}
