using Enemies;
using System.Collections.Generic;
using Entities.Interfaces;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class FirstBossController : EnemyController
{
    [SerializeField] int[] phasesThresholds;

    private States_Base<StateEnum> _idleState;
    private States_Base<StateEnum> _attackState;
    private PhaseSystem _phaseSystem;
    private int _currentPhase = 1;

    private void Start()
    {
        _phaseSystem = new PhaseSystem(phasesThresholds, manager.HealthComponent);
        manager.HealthComponent.OnDamage += CheckPhase;
    }

    protected override void InitializeFsm()
    {
        Fsm = new FSM<StateEnum>();

        var move = GetComponent<IMove>();
        var look = GetComponent<ILook>();
        var attack = GetComponent<IAttack>();

        var idleState = new FirstBossState_Idle<StateEnum>();
        var attackState = new FirstBossState_Attack<StateEnum>();

        _idleState = idleState;
        _attackState = attackState;

        var stateList = new List<States_Base<StateEnum>>
        {
            idleState,
            attackState,
        };

        idleState.AddTransition(StateEnum.Attack, attackState);

        attackState.AddTransition(StateEnum.Idle, idleState);

        foreach (var t in stateList)
        {
            t.Initialize(move, look, attack);
        }

        Fsm.SetInit(idleState);
    }
    protected override void InitializeTree()
    {
        Root.Execute(objectContext);
    }

    void CheckPhase(float damage)
    {
        _currentPhase = _phaseSystem.currentPhase();
        Debug.Log("Current phase is:" + _currentPhase);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
