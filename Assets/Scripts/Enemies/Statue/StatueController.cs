using Interfaces;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] WallFinder _wallFinder;
    [SerializeField] float damage;

    // Variables for the obstacle avoidance.
    [SerializeField] float _radius;
    [SerializeField] float _angle;
    [SerializeField] float _personalArea;
    [SerializeField] int _maxObs;
    [SerializeField] LayerMask _avoidMask;


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
        chaseState = new StatueState_Chase<StateEnum>
        (
            this,
            new Pursuit(transform, target),
            _maxObs,
            _radius,
            _angle,
            _personalArea,
            _avoidMask
        );
        attackState = new StatueState_Attack<StateEnum>(target.transform, damage);
        RunAwayState = new StatueState_Runaway<StateEnum>(_steering);

        var stateList = new List<States_Base<StateEnum>>
        {
            idleState,
            chaseState,
            attackState,
            RunAwayState
        };

        idleState.AddTransition(StateEnum.Runaway, RunAwayState);
        idleState.AddTransition(StateEnum.Chase, chaseState);
        idleState.AddTransition(StateEnum.Attack, attackState);

        chaseState.AddTransition(StateEnum.Attack, attackState);
        chaseState.AddTransition(StateEnum.Idle, idleState);
        chaseState.AddTransition(StateEnum.Runaway, RunAwayState);


        attackState.AddTransition(StateEnum.Idle, idleState);
        attackState.AddTransition(StateEnum.Chase, chaseState);
        attackState.AddTransition(StateEnum.Runaway, RunAwayState);

        RunAwayState.AddTransition(StateEnum.Idle, idleState);
        RunAwayState.AddTransition(StateEnum.Chase, chaseState);
        RunAwayState.AddTransition(StateEnum.Attack, attackState);

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
            RunAwayState.ChangeSteering(new ToPoint(_wallPosition, transform));
            _fsm.Transition(StateEnum.Runaway);
        });
        var aChase = new ActionNode(() =>
        {
            _fsm.Transition(StateEnum.Chase);
        });
        var aAttack = new ActionNode(() => _fsm.Transition(StateEnum.Attack));

        var qCanAttack = new QuestionNode(QuestionCanAttack, aAttack, aChase);
        var qIsTheWallClose = new QuestionNode(QuestionIsTheWallCloseEnough, aIdle, aRunAway);
        var qLookingForWall = new QuestionNode(QuestionIsThereAWall, qIsTheWallClose, aIdle);
        var qTargetInView = new QuestionNode(QuestionTargetInView, qCanAttack, qLookingForWall);
        var qPlayerLookin = new QuestionNode(QuestionIsPlayerLooking, aIdle, qTargetInView);
        _root = qPlayerLookin;
    }


    bool QuestionTargetInView()
    {
        return target != null && _los.LOS(target.transform);
    }
    bool QuestionCanAttack()
    {
        return Vector2.Distance(transform.position, target.position) <= _model.AttackRange;
    }
    bool QuestionIsPlayerLooking()
    {
        return _playerLOS.LOS(target.transform,transform);
    }
    bool QuestionIsThereAWall()
    {
        _wallPosition = _wallFinder.ClosestPoint(Vector2.zero);
        return _wallPosition != Vector2.zero;
    }
    bool QuestionIsTheWallCloseEnough()
    {
        return Vector2.Distance(transform.position, _wallPosition) < .5;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _radius);

        Gizmos.color = Color.red;
        Vector3 dirA = Quaternion.AngleAxis(+_angle / 2, transform.forward) * transform.up;
        Vector3 dirB = Quaternion.AngleAxis(-_angle / 2, transform.forward) * transform.up;
        Gizmos.DrawRay(transform.position, dirA * _radius);
        Gizmos.DrawRay(transform.position, dirB * _radius);
    }
}
