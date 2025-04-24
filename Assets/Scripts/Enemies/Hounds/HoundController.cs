using System.Collections.Generic;
using Enemies.Hounds.States;
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

    [SerializeField] private HoundsCamp camp;
    [SerializeField] private float timeToRested;
    private bool _rested = false;
    
    private float timer = 0f;
    private bool isCounting = false;
    
    private HoundState_Idle<StateEnum> _idleState;
    private HoundState_Patrol<StateEnum> _patrolState;
    
    private Dictionary<AttackType, float> _attacks = new Dictionary<AttackType, float>
    {
        { AttackType.Normal, 60f },
        { AttackType.Charge, 30f },
        { AttackType.Lunge, 10f }
    };
    
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
        
        if (!isCounting) return;
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            isCounting = false;
            timer = 0f;
            _rested = true;
        }
    }
    void InitializedFsm()
    {
        _fsm = new FSM<StateEnum>();
    
        var waypoints = new List<Vector2>();
        for (var i = 0; i < 5; i++)
        {
            waypoints.Add(camp.GetRandomPoint());
        }
        
        var move = GetComponent<IMove>();
        var look = GetComponent<ILook>();
        var attack = GetComponent<IAttack>();
            
        var idleState = new HoundState_Idle<StateEnum>();
        var patrolState = new HoundState_Patrol<StateEnum>(new PatrolToPoint(waypoints, _model.transform));
        var chaseState = new HoundState_Chase<StateEnum>(new Pursuit(_model.transform, target, _model.AttackRange));
        var attackState = new HoundState_Attack<StateEnum>(); //Hay que implementar lo de juani

        _idleState = idleState;
        _patrolState = patrolState;
        
        var stateList = new List<States_Base<StateEnum>>
        {
            idleState,
            patrolState,
            chaseState,
            attackState
        };
    
        idleState.AddTransition(StateEnum.Patrol, patrolState); 
        
        patrolState.AddTransition(StateEnum.Idle, idleState);
        patrolState.AddTransition(StateEnum.Chase, chaseState);
        patrolState.AddTransition(StateEnum.Attack,attackState);
        
        chaseState.AddTransition(StateEnum.Patrol, patrolState);
        chaseState.AddTransition(StateEnum.Attack, attackState);
        
        attackState.AddTransition(StateEnum.Idle, idleState);
        attackState.AddTransition(StateEnum.Patrol, patrolState);
        attackState.AddTransition(StateEnum.Chase, chaseState);
        
        foreach (var t in stateList) 
        { 
            t.Initialize(move, look, attack);
        }
            
        _fsm.SetInit(idleState);
        
    }
    
    void InitializedTree()
    {
        var aIdle = new ActionNode(() =>
        {
            _rested = false;
            _fsm.Transition(input: StateEnum.Idle);
            StartTimer(timeToRested);
        });
        var aPatrol = new ActionNode(() =>
        {
            _fsm.Transition(StateEnum.Patrol);
        });
        var aReturnToCamp = new ActionNode(() =>
        {
            _fsm.Transition(StateEnum.Patrol);
        });
        var aChase = new ActionNode(() =>
        {
            _fsm.Transition(StateEnum.Chase);
        });
        var aAttack = new ActionNode(() => _fsm.Transition(StateEnum.Attack));

        var qFarFromCamp2 = new QuestionNode(QuestionFarFromCamp, aReturnToCamp, aChase);
        var qFarFromCamp = new QuestionNode(QuestionFarFromCamp, aReturnToCamp, aPatrol);
        var qCanAttack = new QuestionNode(QuestionCanAttack, aAttack, qFarFromCamp2);
        var qTargetInView = new QuestionNode(QuestionTargetInView, qCanAttack, qFarFromCamp);
        var qIsRested = new QuestionNode(QuestionIsRested, qTargetInView, aIdle);

        _root = qIsRested;
    }

    bool QuestionIsRested()
    {
        return _rested;
    }
    bool QuestionTargetInView()
    {
        return target != null && _los.LOS(target.transform);
    }
    bool QuestionCanAttack()
    {
        return Vector2.Distance(_model.Position,target.position) <= _model.AttackRange;
    }
    bool QuestionFarFromCamp()
    {
        return camp.IsFarFromCamp(_model.Position);
    }
    
    private void StartTimer(float duration)
    {
        if (isCounting) return;
        timer = duration;
        isCounting = true;
    }
}

