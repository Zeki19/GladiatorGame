using Attack;
using Enemies;
using Enemies.Gaius;
using Entities.StateMachine;
using Unity.Mathematics;
using UnityEngine;

public class ValeriaStateAttack<T> : StatesBase<T>
{
    private ISteering _steering;
    private GameObject _weapon;
    private AttackManager attack;
    private EnemyController controller;
    public ValeriaStateAttack(ISteering steering,GameObject weapon,AttackManager attackManager,EnemyController controller)
    {
        _steering = steering;
        attack = attackManager;
        _weapon = weapon;
        this.controller=controller;
    }

    public override void Enter()
    {
        base.Enter();
        _status.SetStatus(StatusEnum.Attacking,true);
        Vector2 dir = _steering.GetDir();
        _move.Move(Vector2.zero);
        _look.LookDirInsta(dir); 
        _agent._NVagent.isStopped = true;
        attack.ExecuteAttack(controller.currentAttack);
    }

    public override void Execute()
    {
    }

    public override void Exit()
    {
        _weapon.transform.localPosition=new Vector3(0,0.5f,0);
        _weapon.transform.localRotation=quaternion.identity;
        _weapon.SetActive(false);
        _agent._NVagent.isStopped = false;
    }
}