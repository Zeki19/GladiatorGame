using System.Collections;
using System.Collections.Generic;
using Attack;
using Enemies.Attack;
using Entities.StateMachine;
using Unity.Mathematics;
using UnityEngine;

namespace Enemies.Gaius.States
{
    public class GaiusStateAttack<T> : StatesBase<T>
    {
        private ISteering _steering;
        private GameObject _weapon;
        private AttackManager attack;
        private GaiusController controller;
        public GaiusStateAttack(ISteering steering,GameObject weapon,AttackManager attackManager,GaiusController controller)
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
            _weapon.transform.localPosition=new Vector3(0,1f,0);
            _weapon.transform.localRotation=quaternion.identity;
            _weapon.SetActive(false);
            _agent._NVagent.isStopped = false;
            _agent._NVagent.ResetPath(); // clear any current path
            _agent._NVagent.velocity = Vector3.zero; // kill current movement
        }
    }
}
