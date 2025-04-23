using System.Collections.Generic;
using Interfaces;
using UnityEngine;

public class HoundController : MonoBehaviour
{
    public Rigidbody2D target;
    private FSM<StateEnum> _fsm;
    private HoundModel _model;
    private LineOfSight _los;
    private ITreeNode _root;
    private ISteering _steering;

    public float timePred;

    [SerializeField] private HoundsCamp homeCamp;
    
    private void Awake()
    {
        _model = GetComponent<HoundModel>();
        _los = GetComponent<LineOfSight>();
    }
    void Start()
    {
        InitializeSteering();
        InitializedFsm();
        InitializedTree();
    }
    void Update()
    {
        _fsm.OnExecute();
        _root.Execute();
    }

    void InitializeSteering() //No nesecitamos esto, solo el NEW en el State. Pero lo usamos para probar.
    {
        var seek = new Seek(target.transform, _model.transform);
        var flee = new Flee(target.transform, _model.transform);
        var evade = new Evade(_model.transform, target);
        
        _steering = evade;
    }
    void InitializedFsm()
    {
                
        _fsm = new FSM<StateEnum>();
    
        var move = GetComponent<IMove>();
        var look = GetComponent<ILook>();
        var attack = GetComponent<IAttack>();
            
        var idleState = new HoundState_Idle<StateEnum>();
        var patrolState = new HoundState_Patrol<StateEnum>(new Seek(target.transform, _model.transform));
        var attackState = new HoundState_Attack<StateEnum>(target.transform);
        var runawayState = new HoundState_Runaway<StateEnum>(homeCamp.CampCenter);
        var chaseState = new HoundState_Chase<StateEnum>(new Pursuit(_model.transform, target, timePred));
    
        var stateList = new List<States_Base<StateEnum>>
        {
            idleState,
            patrolState,
            attackState,
            runawayState,
            chaseState
        };
    
        idleState.AddTransition(StateEnum.Patrol, patrolState); 
        
        patrolState.AddTransition(StateEnum.Idle, idleState);
        patrolState.AddTransition(StateEnum.Chase, chaseState);
        
        chaseState.AddTransition(StateEnum.Attack, attackState);
        chaseState.AddTransition(StateEnum.Runaway, runawayState);
            
        attackState.AddTransition(StateEnum.Chase, chaseState);
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
        var aIdle = new ActionNode(()=> _fsm.Transition(StateEnum.Idle));
        var aPatrol = new ActionNode(() => _fsm.Transition(StateEnum.Patrol));
        var aAttack = new ActionNode(() => _fsm.Transition(StateEnum.Attack));
        var aRunaway = new ActionNode(() => _fsm.Transition(StateEnum.Runaway));
        var aChase = new ActionNode(() => _fsm.Transition(StateEnum.Chase));
    
        var qFarFromCamp = new QuestionNode(QuestionFarFromCamp, aRunaway, aPatrol);
        var qCanAttack = new QuestionNode(QuestionCanAttack, aAttack, qFarFromCamp);
        var qTargetInView = new QuestionNode(QuestionTargetInView, aChase, aAttack);
        var qLastAction = new QuestionNode(QuestionLastAction, qFarFromCamp, aPatrol);
        
        _root = qTargetInView;
    }
    
    bool QuestionCanAttack()
    {
        return Vector2.Distance(_model.Position,target.position) <= _model.attackRange;
    }
    bool QuestionTargetInView()
    {
        return target != null && _los.LOS(target.transform);
    }
    
    bool QuestionLastAction()
    {
        return false;
    }

    bool QuestionFarFromCamp()
    {
        return homeCamp.IsFarFromCamp(transform.position);
    }
}

