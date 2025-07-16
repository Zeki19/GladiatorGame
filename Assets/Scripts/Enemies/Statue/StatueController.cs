using System.Collections.Generic;
using Enemies;
using Entities.Interfaces;
using Entities.StateMachine;
using UnityEngine;

public class  StatueController : EnemyController
{
    [SerializeField] private float AttackRange;
    //private LineOfSight _los;
    //private LineOfSightNoMono _playerLOS;
    private ISteering _steering;

    public Vector2 _wallPosition;

    [SerializeField] private Transform RotationHandler;
    [SerializeField] float damage;
    [SerializeField] WallFinder _wallFinder;
    [SerializeField] private float LineOfSightRange;

    [Header("Obstacle Avoidance Settings")]
    [SerializeField] float _radius;
    [SerializeField] float _angle;
    [SerializeField] float _personalArea;
    [SerializeField] int _maxObs;
    [SerializeField] LayerMask _avoidMask;


    StatueState_Idle<StateEnum> idleState;
    StatueState_Chase<StateEnum> chaseState;
    StatueState_Attack<StateEnum> attackState;
    StatueState_Runaway<StateEnum> RunAwayState;


    protected override void Awake()
    {
        base.Awake();
        //_los = GetComponent<LineOfSight>();
        _steering = new StToPoint(Vector2.zero, transform);
        //_playerLOS = new LineOfSightNoMono(LineOfSightRange, 90, _avoidMask);
    }

    protected override void InitializeFsm()
    {
        Fsm = new FSM<StateEnum>();

        var move = GetComponent<IMove>();
        var look = GetComponent<ILook>();
        var attack = GetComponent<IAttack>();

        idleState = new StatueState_Idle<StateEnum>(_steering);
        chaseState = new StatueState_Chase<StateEnum>
        (
            this,
            new StPursuit(transform, target, 0),
            _maxObs,
            _radius,
            _angle,
            _personalArea,
            _avoidMask
        );
        attackState = new StatueState_Attack<StateEnum>(target.transform, damage);
        RunAwayState = new StatueState_Runaway<StateEnum>(_steering);

        var stateList = new List<StatesBase<StateEnum>>
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
        Fsm.SetInit(idleState,StateEnum.Idle);
    }

    protected override void InitializeTree()
    {
        var aIdle = new ActionNode(() =>
        {
            idleState.ChangeSteering(new StToPoint(_wallPosition, transform));
            Fsm.Transition(input: StateEnum.Idle);
        });
        var aRunAway = new ActionNode(() =>
        {
            RunAwayState.ChangeSteering(new StToPoint(_wallPosition, transform));
            Fsm.Transition(StateEnum.Runaway);
        });
        var aChase = new ActionNode(() =>
        {
            Fsm.Transition(StateEnum.Chase);
        });
        var aAttack = new ActionNode(() => Fsm.Transition(StateEnum.Attack));
        
        var qCanAttack = new QuestionNode(QuestionCanAttack, aAttack, aChase);
        var qIsTheWallClose = new QuestionNode(QuestionIsTheWallCloseEnough, aIdle, aRunAway);
        var qLookingForWall = new QuestionNode(QuestionIsThereAWall, qIsTheWallClose, aIdle);
        var qTargetInView = new QuestionNode(QuestionTargetInView, qCanAttack, qLookingForWall);
        //var qPlayerLooking = new QuestionNode(QuestionIsPlayerLooking, aIdle, qTargetInView);
        
        //Root = qPlayerLooking;
    }


    bool QuestionTargetInView()
    {
        return false; //target != null && _los.LOS(target.transform);
    }
    bool QuestionCanAttack()
    {
        return Vector2.Distance(transform.position, target.position) <= AttackRange;
    }
    //bool QuestionIsPlayerLooking()
    //{
    //    //return _playerLOS.LOS(RotationHandler.transform,transform, RotationHandler);
    //}
    bool QuestionIsThereAWall()
    {
        _wallPosition = _wallFinder.ClosestPoint(Vector2.zero);
        return _wallPosition != Vector2.zero;
    }
    bool QuestionIsTheWallCloseEnough()
    {
        return Vector2.Distance(transform.position, _wallPosition) < 1;
    }
    void Dead()
    {
        gameObject.SetActive(false);
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
