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

    [SerializeField] private HoundsCamp homeCamp;
    
    //Gova
    private Dictionary<AttackType, float> _attacks = new Dictionary<AttackType, float>
    {
        { AttackType.Normal, 60f },
        { AttackType.Charge, 30f },
        { AttackType.Lunge, 10f }
    };
    
    
    private float timer = 0f;
    private bool isCounting = false;
    
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
        
        if (!isCounting) return;
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            isCounting = false;
            timer = 0f;
            _fsm.Transition(StateEnum.Patrol);
        }
    }

    void InitializeSteering() //No nesecitamos esto, solo el NEW en el State. Pero lo usamos para probar.
    {
        var seek = new Seek(target.transform, _model.transform);
        var patrolToPoint = new PatrolToPoint(homeCamp.GetRandomPatrolPoint(), _model.Position);
        var flee = new Flee(target.transform, _model.transform);
        var evade = new Evade(_model.transform, target);
    }

    private HoundState_Idle<StateEnum> _idleState;
    void InitializedFsm()
    {
                
        _fsm = new FSM<StateEnum>();
    
        var move = GetComponent<IMove>();
        var look = GetComponent<ILook>();
        var attack = GetComponent<IAttack>();
            
        var idleState = new HoundState_Idle<StateEnum>();
        _idleState = idleState;
        var patrolState = new HoundState_Patrol<StateEnum>(new PatrolToPoint(homeCamp.GetRandomPatrolPoint(), _model.Position));
        var attackState = new HoundState_Attack<StateEnum>(target.transform, _model, _attacks, StateEnum.Idle);//Gova
        var runawayState = new HoundState_Runaway<StateEnum>(new PatrolToPoint(homeCamp.CampCenter, _model.Position),homeCamp.CampCenter);
        var chaseState = new HoundState_Chase<StateEnum>(new Pursuit(_model.transform, target, timePred));
    
        var stateList = new List<States_Base<StateEnum>>
        {
            _idleState,
            patrolState,
            attackState,
            runawayState,
            chaseState
        };
    
        idleState.AddTransition(StateEnum.Patrol, patrolState); 
        
        patrolState.AddTransition(StateEnum.Idle, idleState);
        patrolState.AddTransition(StateEnum.Chase, chaseState);
        patrolState.AddTransition(StateEnum.Runaway,runawayState);
        
        chaseState.AddTransition(StateEnum.Attack, attackState);
        chaseState.AddTransition(StateEnum.Runaway, runawayState);
            
        attackState.AddTransition(StateEnum.Chase, chaseState);
        attackState.AddTransition(StateEnum.Runaway, runawayState);
        attackState.AddTransition(StateEnum.Idle, idleState);//Gova
        
        runawayState.AddTransition(StateEnum.Idle, idleState);
        
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
            StartTimer(); //already preset in 4sec
        });
        var aPatrol = new ActionNode(() =>
        {
            _fsm.Transition(StateEnum.Patrol);
            //StartTimer(8);
        });
        var aRunaway = new ActionNode(() => _fsm.Transition(StateEnum.Runaway));
        var aChase = new ActionNode(() => _fsm.Transition(StateEnum.Chase));
        var aAttack = new ActionNode(() => _fsm.Transition(StateEnum.Attack));//Gova
        
        var qFarFromCamp = new QuestionNode(QuestionFarFromCamp, aRunaway, aPatrol);
        var qCanAttack = new QuestionNode(QuestionCanAttack, aAttack, aChase);
        var qTargetInView = new QuestionNode(QuestionTargetInView, qCanAttack, qFarFromCamp);
        var qIsStillIdle = new QuestionNode(QuestionIsStillIdle, aIdle, qTargetInView);

        _root = qIsStillIdle;
    }


    bool QuestionCanAttack()
    {
        return Vector2.Distance(_model.Position,target.position) <= _model.attackRange;
    }
    bool QuestionTargetInView()
    {
        return target != null && _los.LOS(target.transform);
    }
    
    bool QuestionIsStillIdle()
    {
        return _fsm.CurrentState() == _idleState;
    }

    bool QuestionFarFromCamp()
    {
        return homeCamp.IsFarFromCamp(transform.position);
    }
    
    private void StartTimer(float duration = 4f)
    {
        if (isCounting) return;
        timer = duration;
        isCounting = true;
    }
}

