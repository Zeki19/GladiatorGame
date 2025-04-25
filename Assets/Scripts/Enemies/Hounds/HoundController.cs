using System.Collections;
using System.Collections.Generic;
using Enemies.Hounds.States;
using Interfaces;
using UnityEngine;

public class HoundController : MonoBehaviour
{
    public Rigidbody2D target;
    private FSM<StateEnum> _fsm;
    private HoundModel _model;
    private HoundView _view;
    private LineOfSight _los;
    private ITreeNode _root;
    private ISteering _steering;

    [SerializeField] private HoundsCamp camp;
    [SerializeField] private float idleDuration;
    [SerializeField] private float patrolDuration;
    [SerializeField] private float attackCooldown;

    private HoundState_Idle<StateEnum> _idleState;
    private HoundState_Patrol<StateEnum> _patrolState;
    private HoundState_Chase<StateEnum> _chaseState;
    private HoundState_Attack<StateEnum> _attackState;
    private HoundState_Runaway<StateEnum> _runawayState;
    private ISteering _patrolSteering;
    private ISteering _pursuitSteering;
    private ISteering _runawaySteering;

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

    void InitalizeSteering()
    {
        //Falta VAR para settear la cantidad de waypoints, actualmente usamos 5.
        var waypoints = new List<Vector2>();
        for (var i = 0; i < 5; i++)
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

        var idleState = new HoundState_Idle<StateEnum>(_view);
        var patrolState = new HoundState_Patrol<StateEnum>(_patrolSteering, _view);
        var chaseState = new HoundState_Chase<StateEnum>(_pursuitSteering, _view);
        var attackState = new HoundState_Attack<StateEnum>(target.transform, _model, _attacks, StateEnum.Idle, _view);
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
        idleState.AddTransition(StateEnum.Runaway, runawayState);

        patrolState.AddTransition(StateEnum.Idle, idleState);
        patrolState.AddTransition(StateEnum.Chase, chaseState);
        patrolState.AddTransition(StateEnum.Runaway, runawayState);

        chaseState.AddTransition(StateEnum.Patrol, patrolState);
        chaseState.AddTransition(StateEnum.Attack, attackState);
        chaseState.AddTransition(StateEnum.Runaway, runawayState);

        attackState.AddTransition(StateEnum.Idle, idleState);
        attackState.AddTransition(StateEnum.Chase, chaseState);
        attackState.AddTransition(StateEnum.Runaway, runawayState);
        
        runawayState.AddTransition(StateEnum.Patrol, patrolState);

        foreach (var t in stateList)
        {
            t.Initialize(move, look, attack);
        }

        _fsm.SetInit(idleState);
        StartCoroutine(RestFor());
    }

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
            _fsm.Transition(StateEnum.Runaway); 
        });
        var aChase = new ActionNode(() => { _fsm.Transition(StateEnum.Chase); });
        var aAttack = new ActionNode(() =>
        {
            if (_fsm.CurrentState() == _attackState) return;
            _fsm.Transition(StateEnum.Attack);
            StartCoroutine(AttackCD());
        });

        var qInCamp = new QuestionNode(QuestionIsInCamp, aPatrol, aRunaway);
        var qRunawayState = new QuestionNode(QuestionIsRunaway, qInCamp, aIdle);
        var qAttackCd = new QuestionNode(QuestionIsAttackCD, aChase, aAttack);
        var qAttackRange = new QuestionNode(QuestionAttackRange, qAttackCd, aChase);
        var qStateAttack = new QuestionNode(QuestionIsAttack, qAttackCd, qRunawayState);
        var qFarFromLimit = new QuestionNode(QuestionFarFromCamp, qAttackRange, qAttackRange);
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

    private bool _isRested = false;
    private bool _isTired = false;
    private bool _attackCd = false;

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

    bool QuestionIsAttackCD()
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

    IEnumerator AttackCD()
    {
        yield return new WaitForSeconds(attackCooldown);
        _attackCd = false;
    }
}

