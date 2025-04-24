using Enemies.Hounds.States;
using Interfaces;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class StatueController : MonoBehaviour
{
    public Rigidbody2D target;
    private FSM<StateEnum> _fsm;
    private StatueModel _model;
    private LineOfSight _los;
    private ITreeNode _root;
    private LineOfSightNoMono _playerLOS;
    private ISteering _steering;
    public Vector2 _wallPosition;
    [SerializeField] ObstacleAvoidance _obstacleAvoidance;


    StatueState_Idle<StateEnum> idleState;
    StatueState_Chase<StateEnum> chaseState;
    StatueState_Attack<StateEnum> attackState;
    StatueState_Runaway<StateEnum> RunAwayState;
    private void Awake()
    {
        _model = GetComponent<StatueModel>();
        _los = GetComponent<LineOfSight>();

        _playerLOS = new LineOfSightNoMono();
        _playerLOS.angle = 90;
        _playerLOS.range = 3;
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

        idleState = new StatueState_Idle<StateEnum>();
        chaseState = new StatueState_Chase<StateEnum>(new Pursuit(_model.transform, target, _model.AttackRange));
        attackState = new StatueState_Attack<StateEnum>();
        RunAwayState = new StatueState_Runaway<StateEnum>(_steering);

        var stateList = new List<States_Base<StateEnum>>
        {
            idleState,
            chaseState,
            attackState,
            RunAwayState
        };

        idleState.AddTransition(StateEnum.Runaway, RunAwayState);

        chaseState.AddTransition(StateEnum.Attack, attackState);

        attackState.AddTransition(StateEnum.Idle, idleState);
        attackState.AddTransition(StateEnum.Chase, chaseState);

        RunAwayState.AddTransition(StateEnum.Idle, idleState);

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
            _fsm.Transition(input: StateEnum.Idle);
        });
        var aPatrol = new ActionNode(() =>
        {
            _fsm.Transition(StateEnum.Patrol);
        });
        var aRunAway = new ActionNode(() =>
        {
            RunAwayState.ChangeSteering(new ToPoint(_wallPosition, transform.position));
            _fsm.Transition(StateEnum.Runaway);
        });
        var aChase = new ActionNode(() =>
        {
            _fsm.Transition(StateEnum.Chase);
        });
        var aAttack = new ActionNode(() => _fsm.Transition(StateEnum.Attack));

        var qCanAttack = new QuestionNode(QuestionCanAttack, aAttack, aChase);
        var qLookingForWall = new QuestionNode(QuestionIsThereAWall, aRunAway, aIdle);
        var qTargetInView = new QuestionNode(QuestionTargetInView, qCanAttack, aIdle);
        var qPlayerLookin = new QuestionNode(QuestionIsPlayerLooking, aIdle, qLookingForWall);
        _root = qPlayerLookin;
    }


    bool QuestionTargetInView()
    {
        return target != null && _los.LOS(target.transform);
    }
    bool QuestionCanAttack()
    {
        return Vector2.Distance(_model.transform.position, target.position) <= _model.AttackRange;
    }
    bool QuestionIsPlayerLooking()
    {
        if (_playerLOS.LOS(target.transform, transform)) Debug.Log("ME ESTAN MIRANDO GUEY");
        return _playerLOS.LOS(target.transform,transform);
    }
    bool QuestionIsThereAWall()
    {
        _wallPosition = _obstacleAvoidance.ClosestPoint(Vector2.zero);
        return _wallPosition != Vector2.zero;
    }
}
