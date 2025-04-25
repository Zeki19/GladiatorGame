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
    [SerializeField] private float timeToTired;
    private bool _rested = false;
    private bool _tired = false;
    
    private float timer = 0f;
    private bool isCounting = false;
    
    private HoundState_Idle<StateEnum> _idleState;
    private HoundState_Patrol<StateEnum> _patrolState;
    private HoundState_Chase<StateEnum> _chaseState;
    private HoundState_Attack<StateEnum> _attackState;
    private ISteering _patrolSteering;
    private ISteering _pursuitSteering;
    private ISteering _runawaySteering;
    private HoundView _view;
    
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
        _view = GetComponent<HoundView>();
    }
    void Start()
    {
        InitalizeSteering();
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
            _tired = true;
        }
    }

    void InitalizeSteering()
    {
        var waypoints = new List<Vector2>();
        for (var i = 0; i < 5; i++)
        {
            waypoints.Add(camp.GetRandomPoint());
        }

        _patrolSteering = new PatrolToWaypoints(waypoints, _model.transform);
        _runawaySteering = new ToPoint(camp.CampCenter, _model.gameObject.transform.position);
        _pursuitSteering = new Pursuit(_model.transform, target, _model.AttackRange);
    }
    void InitializedFsm()
    {
        _fsm = new FSM<StateEnum>();
    

        
        var move = GetComponent<IMove>();
        var look = GetComponent<ILook>();
        var attack = GetComponent<IAttack>();
            
        var idleState = new HoundState_Idle<StateEnum>(_view);
        var patrolState = new HoundState_Patrol<StateEnum>(_patrolSteering,_view);
        var chaseState = new HoundState_Chase<StateEnum>(_pursuitSteering,_view);
        var attackState = new HoundState_Attack<StateEnum>(target.transform, _model, _attacks, StateEnum.Idle,_view);

        _idleState = idleState;
        _patrolState = patrolState;
        _chaseState = chaseState;
        _attackState = attackState;
        
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
            _fsm.Transition(StateEnum.Idle);
            StartTimer(timeToRested);
        });
        var aPatrol = new ActionNode(() =>
        {
            _tired = false;
            //_patrolState.ChangeSteering(_patrolSteering);
            _fsm.Transition(StateEnum.Patrol);
            StartTimer(timeToTired);
        });
        var aReturnToCamp = new ActionNode(() =>
        {
            _patrolState.ChangeSteering(new ToPoint(camp.CampCenter, _model.Position));
            _fsm.Transition(StateEnum.Patrol);
        });
        var aChase = new ActionNode(() =>
        {
            _fsm.Transition(StateEnum.Chase);
        });
        var aAttack = new ActionNode(() => _fsm.Transition(StateEnum.Attack));

        
        var qLostTarget = new QuestionNode(QuestionHasLostTargetForTooLong, aReturnToCamp, aChase);
        var qCanAttack = new QuestionNode(QuestionCanAttack, aAttack,aChase);
        var qStateAttack = new QuestionNode(QuestionIsAttack, qCanAttack, aIdle);
        var qTargetInViewChase = new QuestionNode(QuestionTargetInView, qCanAttack, qLostTarget);
        var qStateChase = new QuestionNode(QuestionIsChase, qTargetInViewChase, qStateAttack);
        var qIsTired = new QuestionNode(QuestionIsTired, aIdle, aPatrol);
        var qTargetInViewPatrol = new QuestionNode(QuestionTargetInView, aChase, qIsTired);
        var qStatePatrol = new QuestionNode(QuestionIsPatrol, qTargetInViewPatrol, qStateChase);
        var qIsRested = new QuestionNode(QuestionIsRested, aPatrol, aIdle);
        var qStateIdle = new QuestionNode(QuestionIsIdle, qIsRested, qStatePatrol);
        var qFarFromCamp = new QuestionNode(QuestionFarFromCamp, aReturnToCamp, qStateIdle);

        _root = qFarFromCamp;
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

    bool QuestionIsIdle()
    {
        return _fsm.CurrentState() == _idleState;
    }
    bool QuestionIsPatrol()
    {
        return _fsm.CurrentState() == _patrolState;
    }
    bool QuestionIsChase()
    {
        return _fsm.CurrentState() == _chaseState;
    }
    bool QuestionIsAttack()
    {
        return _fsm.CurrentState() == _attackState;
    }

    bool QuestionIsTired()
    {
        return _tired;
    }

    bool QuestionHasLostTargetForTooLong()
    {
        return false;
    }
    
    private void StartTimer(float duration)
    {
        if (isCounting) return;
        timer = duration;
        isCounting = true;
    }
}

