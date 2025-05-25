using System.Collections;
using System.Collections.Generic;
using Enemies.Hounds.States;
using Entities.Interfaces;
using NUnit.Framework;
using UnityEngine;

public class HoundController : EnemyController
{
    [Header("Required GameObjects")]
    [SerializeField] private HoundsCamp camp;

    [SerializeField] private float AmountOfWaypoints;
    
    [Header("States Settings")]
    [Tooltip("Time it takes to force a state change.")]
    [SerializeField] private float idleDuration;
    [SerializeField] private float patrolDuration;
    [SerializeField] private float AttackCooldown;
    [SerializeField] private float AttackRange;

    [Header("Obstacle Avoidance Settings")]
    [SerializeField] public int _maxObs;
    [SerializeField] public float _radius;
    [SerializeField] public float _angle;
    [SerializeField] public float _personalArea;
    [SerializeField] public LayerMask _obsMask;

    private Vector2 _targetLastPos;

    #region Private Variables
    
    private LineOfSight _los;
    private ISteering _steering;
    private HoundState_Idle<StateEnum> _idleState;
    private HoundState_Patrol<StateEnum> _patrolState;
    private HoundState_Chase<StateEnum> _chaseState;
    private HoundState_Attack<StateEnum> _attackState;
    private HoundState_Runaway<StateEnum> _runawayState;
    private HoundState_Search<StateEnum> _searchState;
    private ISteering _patrolSteering;
    private ISteering _pursuitSteering;
    private ISteering _runawaySteering;
    private ISteering _toPointSteering;
    private StObstacleAvoidance _avoidWalls;
    
    #endregion
    
    private Dictionary<AttackType, float> _attacks = new Dictionary<AttackType, float>
    {
        { AttackType.Normal, 60f },
        { AttackType.Charge, 30f },
        { AttackType.Lunge, 10f }
    };

    protected override void Awake()
    {
        base.Awake();
        _los = GetComponent<LineOfSight>();
        
        _avoidWalls = new StObstacleAvoidance(_maxObs, _radius, _angle, _personalArea, _obsMask);
    }

    protected void Start()
    {
        InitalizeSteering();
    }
    

    #region States
    void InitalizeSteering()
    {
        var waypoints = new List<Vector2>();
        for (var i = 0; i < AmountOfWaypoints; i++)
        {
            waypoints.Add(camp.GetRandomPoint());
        }

        //No hace falta inicializarlo asi
        _patrolSteering = new StPatrolToWaypoints(waypoints, manager.model.transform);
        _runawaySteering = new StToPoint(camp.CampCenter, manager.model.transform);
        _pursuitSteering = new StPursuit(manager.model.transform, target);
        _toPointSteering = new StToPoint(_targetLastPos, manager.model.transform);
    }

    protected override void InitializeFsm()
    {
        Fsm = new FSM<StateEnum>();

        var move = GetComponent<IMove>();
        var look = GetComponent<ILook>();
        var attack = GetComponent<IAttack>();

        var idleState = new HoundState_Idle<StateEnum>(this, idleDuration);
        var patrolState = new HoundState_Patrol<StateEnum>(_patrolSteering, _avoidWalls, transform, this, patrolDuration);
        var chaseState = new HoundState_Chase<StateEnum>(_pursuitSteering, _avoidWalls, transform,target.transform);
        var searchState = new HoundState_Search<StateEnum>(_toPointSteering, _avoidWalls, manager.model.transform, this);
        var attackState = new HoundState_Attack<StateEnum>(target.transform, manager.model, _attacks, this, AttackCooldown);
        var runawayState = new HoundState_Runaway<StateEnum>(_runawaySteering, _avoidWalls, transform);

        _idleState = idleState;
        _patrolState = patrolState;
        _chaseState = chaseState;
        _searchState = searchState;
        _attackState = attackState;
        _runawayState = runawayState;
        
        var stateList = new List<States_Base<StateEnum>>
        {
            idleState,
            patrolState,
            chaseState,
            searchState,
            attackState,
            runawayState
        };

        idleState.AddTransition(StateEnum.Patrol, patrolState);

        patrolState.AddTransition(StateEnum.Idle, idleState);
        patrolState.AddTransition(StateEnum.Chase, chaseState);
        
        chaseState.AddTransition(StateEnum.Attack, attackState);
        chaseState.AddTransition(StateEnum.Runaway, runawayState);
        chaseState.AddTransition(StateEnum.Search, searchState);
        
        searchState.AddTransition(StateEnum.Chase, chaseState);
        searchState.AddTransition(StateEnum.Runaway, runawayState);
        
        attackState.AddTransition(StateEnum.Chase, chaseState);
        attackState.AddTransition(StateEnum.Attack, attackState);
        
        runawayState.AddTransition(StateEnum.Patrol, patrolState);

        foreach (var t in stateList)
        {
            t.Initialize(move, look, attack);
        }

        Fsm.SetInit(idleState);
    }
    

    #endregion

    #region DecisionTree
    
    protected override void InitializeTree()
    {
        var aIdle = new ActionNode(() =>
        {
            Fsm.Transition(StateEnum.Idle);
        });
        
        var aPatrol = new ActionNode(() =>
        {
            Fsm.Transition(StateEnum.Patrol);
        });
        
        var aRunaway = new ActionNode(() =>
        {
            Fsm.Transition(StateEnum.Runaway); 
        });
        
        var aChase = new ActionNode(() =>
        {
            Fsm.Transition(StateEnum.Chase);
        });
        
        var aSearch = new ActionNode(() =>
        {
            _searchState.ChangeSteering(new StToPoint(_chaseState.lastSeenPositionOfTarget, manager.model.transform));
            Fsm.Transition(StateEnum.Search);
        });
        
        var aAttack = new ActionNode(() =>
        {
            Fsm.Transition(StateEnum.Attack);
        });



        var qInCamp = new QuestionNode(QuestionIsInCamp, aPatrol, aRunaway);
        var qCanAttack  = new QuestionNode(QuestionIsAttackInCd, aChase, aAttack);
        var qAttackRange = new QuestionNode(QuestionAttackRange, qCanAttack, aChase);
        var qStateAttack = new QuestionNode(QuestionIsAttack, qAttackRange, qInCamp);
        var qFinishedSearching = new QuestionNode(QuestionFinishedSearching, aRunaway, aSearch);
        var qTargetInViewSearch = new QuestionNode(QuestionTargetInView, aChase, qFinishedSearching);
        var qStateSearch = new QuestionNode(QuestionIsSearch, qTargetInViewSearch, qStateAttack);
        var qFarFromLimit = new QuestionNode(QuestionFarFromCamp, aRunaway, qAttackRange);
        var qTargetInViewChase = new QuestionNode(QuestionTargetInView, qFarFromLimit, aSearch);
        var qStateChase = new QuestionNode(QuestionIsChase, qTargetInViewChase, qStateSearch);
        var qIsTired = new QuestionNode(QuestionIsTired, aIdle, aPatrol);
        var qTargetInViewPatrol = new QuestionNode(QuestionTargetInView, aChase, qIsTired);
        var qStatePatrol = new QuestionNode(QuestionIsPatrol, qTargetInViewPatrol, qStateChase);
        var qIsRested = new QuestionNode(QuestionIsRested, aPatrol, aIdle);
        var qStateIdle = new QuestionNode(QuestionIsIdle, qIsRested, qStatePatrol);
        var qFarFromCamp = new QuestionNode(QuestionFarFromCamp, aRunaway, qStateIdle);

        //Root = qFarFromCamp;
    }

    bool QuestionFinishedSearching()
    {
        return _searchState.Searched;
    }
    bool QuestionFarFromCamp()
    {
        return camp.IsFarFromCamp(manager.model.Position);
    }
    bool QuestionIsInCamp()
    {
        return camp.IsInCamp(manager.model.Position);
    }
    bool QuestionIsRested()
    {
        return _idleState.FinishedResting;
    }
    bool QuestionTargetInView()
    {
        return target != null && _los.LOS(target.transform);
    }
    bool QuestionIsTired()
    {
        return _patrolState.TiredOfPatroling;
    }
    bool QuestionAttackRange()
    {
        return Vector2.Distance(manager.model.Position, target.position) <= AttackRange;
    }
    bool QuestionIsAttackInCd()
    {
        return _attackState.canAttack;
    }
    bool QuestionIsIdle()
    {
        return Fsm.CurrentState() == _idleState;
    }
    bool QuestionIsPatrol()
    {
        return Fsm.CurrentState() == _patrolState;
    }
    bool QuestionIsChase()
    {
        return Fsm.CurrentState() == _chaseState;
    }
    bool QuestionIsAttack()
    {
        return Fsm.CurrentState() == _attackState;
    }
    bool QuestionIsSearch()
    {
        return Fsm.CurrentState() == _searchState;
    }
    
    #endregion

}

