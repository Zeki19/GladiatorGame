using Enemies;
using Interfaces;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class FirstBossController : EnemyController
{
    [SerializeField] TreeNodeSO RootSO;
    AIContext objectContext;
    EnemyManager selfManager;

    private States_Base<StateEnum> _idleState;
    private States_Base<StateEnum> _patrolState;
    private States_Base<StateEnum> _chaseState;
    private States_Base<StateEnum> _searchState;
    private States_Base<StateEnum> _attackState;
    private States_Base<StateEnum> _runawayState;
    private void Awake()
    {
        selfManager = ServiceLocator.Instance.GetService<EnemiesManager>().GetManager(gameObject);
    }


    protected override void InitializeFsm()
    {
        Fsm = new FSM<StateEnum>();

        var move = GetComponent<IMove>();
        var look = GetComponent<ILook>();
        var attack = GetComponent<IAttack>();

        var idleState = new FirstBossState_Idle<StateEnum>();
        var patrolState = new FirstBossState_Patrol<StateEnum>();
        var chaseState = new FirstBossState_Chase<StateEnum>();
        var searchState = new FirstBossState_Search<StateEnum>();
        var attackState = new FirstBossState_Attack<StateEnum>();
        var runawayState = new FirstBossState_Runaway<StateEnum>();

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
    protected override void Update()
    {
        AIContext context = new AIContext //Actualizar para dar refencias en vez de ir actualizando.
        {
            selfTransform = transform,
            playerTransform = target.gameObject.transform,
            attackRange = attackRange,
            stateMachine = Fsm
        };
        base.Update();

    }
    protected override void InitializeTree()
    {
        RootSO.Execute(objectContext);
    }
}
