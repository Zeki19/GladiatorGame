using System.Collections;
using System.Collections.Generic;
using Enemies.Hounds.States;
using Interfaces;
using UnityEngine;

public class HoundController : MonoBehaviour
{
    [Header("Required GameObjects")]
    [SerializeField] private Rigidbody2D target;
    [SerializeField] private HoundsCamp camp;
    [Header("States Settings")]
    [Tooltip("Time it takes to force a state change.")]
    [SerializeField] private float idleDuration;
    [SerializeField] private float patrolDuration;
    
    #region Private Variables
    
    private FSM<StateEnum> _fsm;
    private HoundModel _model;
    private HoundView _view;
    private LineOfSight _los;
    private ITreeNode _root;
    private ISteering _steering;
    private HoundState_Idle<StateEnum> _idleState;
    private HoundState_Patrol<StateEnum> _patrolState;
    private HoundState_Chase<StateEnum> _chaseState;
    private HoundState_Attack<StateEnum> _attackState;
    private HoundState_Runaway<StateEnum> _runawayState;
    private ISteering _patrolSteering;
    private ISteering _pursuitSteering;
    private ISteering _runawaySteering;
    
    #endregion
    
    private Dictionary<AttackType, float> _attacks = new Dictionary<AttackType, float>
    {
        { AttackType.Normal, 60f },
        { AttackType.Charge, 30f },
        { AttackType.Lunge, 10f }
    };

    private void Awake()
    {
        _model = GetComponent<HoundModel>();
        _view = GetComponent<HoundView>();
        _los = GetComponent<LineOfSight>();
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
    }

    #region States
    void InitalizeSteering()
    {
        var waypoints = new List<Vector2>();
        for (var i = 0; i < _model.AmountOfWaypoints; i++)
        {
            waypoints.Add(camp.GetRandomPoint());
        }

        _patrolSteering = new PatrolToWaypoints(waypoints, _model.transform);
        _runawaySteering = new ToPoint(camp.CampCenter, _model.transform);
        _pursuitSteering = new Pursuit(_model.transform, target);
    }

    void InitializedFsm()
    {
        _fsm = new FSM<StateEnum>();

        var move = GetComponent<IMove>();
        var look = GetComponent<ILook>();
        var attack = GetComponent<IAttack>();

        var idleState = new HoundState_Idle<StateEnum>();
        var patrolState = new HoundState_Patrol<StateEnum>(_patrolSteering);
        var chaseState = new HoundState_Chase<StateEnum>(_pursuitSteering);
        var attackState = new HoundState_Attack<StateEnum>(target.transform, _model, _attacks, StateEnum.Idle);
        var runawayState = new HoundState_Runaway<StateEnum>(_runawaySteering);

        _idleState = idleState;
        _patrolState = patrolState;
        _chaseState = chaseState;
        _attackState = attackState;
        _runawayState = runawayState;
        
        var stateList = new List<States_Base<StateEnum>>
        {
            idleState,
            patrolState,
            chaseState,
            attackState,
            runawayState
        };

        idleState.AddTransition(StateEnum.Patrol, patrolState);

        patrolState.AddTransition(StateEnum.Idle, idleState);
        patrolState.AddTransition(StateEnum.Chase, chaseState);
        
        chaseState.AddTransition(StateEnum.Attack, attackState);
        chaseState.AddTransition(StateEnum.Runaway, runawayState);
        
        attackState.AddTransition(StateEnum.Chase, chaseState);
        attackState.AddTransition(StateEnum.Attack, attackState);
        
        runawayState.AddTransition(StateEnum.Patrol, patrolState);

        foreach (var t in stateList)
        {
            t.Initialize(move, look, attack);
        }

        _fsm.SetInit(idleState);
        StartCoroutine(RestFor());
    }
    

    #endregion

    #region DecisionTree
    
    private bool _isRested = false;
    private bool _isTired = false;
    private bool _attackCd = false;
    
    void InitializedTree()
    {
        var aIdle = new ActionNode(() =>
        {
            if (_fsm.CurrentState() == _idleState) return;
            _fsm.Transition(StateEnum.Idle);
            StartCoroutine(RestFor());
        });
        
        var aPatrol = new ActionNode(() =>
        {
            if (_fsm.CurrentState() == _patrolState) return;
            _fsm.Transition(StateEnum.Patrol);
            StartCoroutine(PatrolFor());
        });
        
        var aRunaway = new ActionNode(() =>
        {
            if (_fsm.CurrentState() == _runawayState) return;
            _fsm.Transition(StateEnum.Runaway); 
        });
        
        var aChase = new ActionNode(() =>
        {
            if (_fsm.CurrentState() == _chaseState) return;
            _fsm.Transition(StateEnum.Chase);
            StopCoroutine(PatrolFor());
        });
        
        var aAttack = new ActionNode(() =>
        {
            if (_fsm.CurrentState() == _attackState) return;
            _fsm.Transition(StateEnum.Attack);
            StartCoroutine(AttackCd());
        });

        var qInCamp = new QuestionNode(QuestionIsInCamp, aPatrol, aRunaway);
        var qStateRunaway = new QuestionNode(QuestionIsRunaway, qInCamp, aRunaway);
        var qCanAttack  = new QuestionNode(QuestionIsAttackInCd, aChase, aAttack);
        var qAttackRange = new QuestionNode(QuestionAttackRange, qCanAttack, aChase);
        var qStateAttack = new QuestionNode(QuestionIsAttack, qAttackRange, qStateRunaway);
        var qFarFromLimit = new QuestionNode(QuestionFarFromCamp, aRunaway, qAttackRange);
        var qTargetInViewChase = new QuestionNode(QuestionTargetInView, qFarFromLimit, aRunaway);
        var qStateChase = new QuestionNode(QuestionIsChase, qTargetInViewChase, qStateAttack);
        var qIsTired = new QuestionNode(QuestionIsTired, aIdle, aPatrol);
        var qTargetInViewPatrol = new QuestionNode(QuestionTargetInView, aChase, qIsTired);
        var qStatePatrol = new QuestionNode(QuestionIsPatrol, qTargetInViewPatrol, qStateChase);
        var qIsRested = new QuestionNode(QuestionIsRested, aPatrol, aIdle);
        var qStateIdle = new QuestionNode(QuestionIsIdle, qIsRested, qStatePatrol);
        var qFarFromCamp = new QuestionNode(QuestionFarFromCamp, aRunaway, qStateIdle);

        _root = qFarFromCamp;
    }
    
    bool QuestionFarFromCamp()
    {
        return camp.IsFarFromCamp(_model.Position);
    }
    bool QuestionIsInCamp()
    {
        return camp.IsInCamp(_model.Position);
    }
    bool QuestionIsRested()
    {
        return _isRested;
    }
    bool QuestionTargetInView()
    {
        return target != null && _los.LOS(target.transform);
    }
    bool QuestionIsTired()
    {
        return _isTired;
    }
    bool QuestionAttackRange()
    {
        return Vector2.Distance(_model.Position, target.position) <= _model.AttackRange;
    }
    bool QuestionIsAttackInCd()
    {
        return _attackCd;
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
    bool QuestionIsRunaway()
    {
        return _fsm.CurrentState() == _runawayState;
    }
    
    IEnumerator RestFor()
    {
        _isRested = false;
        yield return new WaitForSeconds(idleDuration);
        _isRested = true;
    }
    IEnumerator PatrolFor()
    {
        _isTired = false;
        yield return new WaitForSeconds(patrolDuration);
        _isTired = true;
    }

    IEnumerator AttackCd()
    {
        _attackCd = true;
        yield return new WaitForSeconds(_model.AttackCooldown);
        _attackCd = false;
    }
    
    #endregion

}

