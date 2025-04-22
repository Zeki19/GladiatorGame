using System.Collections.Generic;
using Interfaces;
using UnityEngine;

public class HoundController : MonoBehaviour
{
    public Transform target;
    private FSM<StateEnum> _fsm;
    private HoundModel _model;
    private LineOfSight _los;
    private ITreeNode _root;
    
    private void Awake()
    {
        _model = GetComponent<HoundModel>();
        _los = GetComponent<LineOfSight>();
    }
    void Start()
    {
        InitializedFsm();
        InitializedTree();
    }
    void Update()
    {
        _fsm.OnExecute();
        _root.Execute();
    }
    void InitializedFsm()
    {
                
        _fsm = new FSM<StateEnum>();
    
        var move = GetComponent<IMove>();
        var look = GetComponent<ILook>();
        var attack = GetComponent<IAttack>();
            
        var idleState = new HoundState_Idle<StateEnum>();
        var patrolState = new HoundState_Patrol<StateEnum>();
        var attackState = new HoundState_Attack<StateEnum>(target);
        var runawayState = new HoundState_Runaway<StateEnum>();
        
    
        var stateList = new List<States_Base<StateEnum>>
        {
            idleState,
            patrolState,
            attackState,
            runawayState
        };
    
        idleState.AddTransition(StateEnum.Patrol, patrolState); 
        
        patrolState.AddTransition(StateEnum.Idle, idleState);
        patrolState.AddTransition(StateEnum.Attack, attackState);
            
        attackState.AddTransition(StateEnum.Patrol, patrolState);
        attackState.AddTransition(StateEnum.Runaway, runawayState);
        
        runawayState.AddTransition(StateEnum.Idle, idleState);
        
        foreach (var t in stateList) 
        { 
            t.Initialize(move, look, attack);
        }
            
        _fsm.SetInit(patrolState);
    }
    
    void InitializedTree()
    {
        var aIdle = new ActionNode(()=>_fsm.Transition(StateEnum.Idle));
        var aPatrol = new ActionNode(() => _fsm.Transition(StateEnum.Patrol));
        var aAttack = new ActionNode(() => _fsm.Transition(StateEnum.Attack));
        var aRunaway = new ActionNode(() => _fsm.Transition(StateEnum.Runaway));
    
        var qFarFromCamp = new QuestionNode(QuestionFarFromCamp, aRunaway, aAttack);
        var qCanAttack = new QuestionNode(QuestionCanAttack, aAttack, qFarFromCamp);
        var qTargetInView = new QuestionNode(QuestionTargetInView, qCanAttack, aIdle);
        var qLastAction = new QuestionNode(QuestionLastAction, qFarFromCamp, aPatrol);
        
        _root = qTargetInView;
    }
    
    bool QuestionCanAttack()
    {
        return false;
    }
    bool QuestionTargetInView()
    {
        return false;
    }
    
    bool QuestionLastAction()
    {
        return false;
    }

    bool QuestionFarFromCamp()
    {
        return false;
    }
}

