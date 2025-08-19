using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using Entities.Interfaces;
using Entities.StateMachine;
using Player.PlayerStates;
using UnityEngine;

namespace Player
{
    public class PlayerController : EntityController
    {
        public PhaseSystem _phaseSystem;
        private FSM<StateEnum> Fsm;

        void Dead()
        {
            transform.parent.gameObject.SetActive(false);
            manager.PlaySound("Death", "Player");
            ServiceLocator.Instance.GetService<SceneChanger>().ChangeScene(3);
        }

        private void Start()
        {
            manager.HealthComponent.OnDead += Dead;
            var playerManager = manager as PlayerManager;
            _phaseSystem = new PhaseSystem(playerManager.stats.stateThreshold, manager.HealthComponent);
            InitializeFsm();
        }

        protected virtual void Update()
        {
            Fsm.OnExecute();
        }

        protected override void InitializeFsm()
        {
            Fsm = new FSM<StateEnum>();


            var stateList = new List<State<StateEnum>>();

            var idleState = new PSIdle<StateEnum>(StateEnum.Walk);
            var walkState = new PSWalk<StateEnum>(StateEnum.Idle);
            var baseAttackState = new PSAttack<StateEnum>(StateEnum.Idle, (PlayerManager)manager);
            var changeAttackState = new PSChargeAttack<StateEnum>(StateEnum.Idle, (PlayerManager)manager);
            var dashState = new PSDash<StateEnum>(StateEnum.Idle, (PlayerManager)manager, this);

            idleState.AddTransition(StateEnum.Walk, walkState);
            idleState.AddTransition(StateEnum.Dash, dashState);
            idleState.AddTransition(StateEnum.Attack, baseAttackState);
            idleState.AddTransition(StateEnum.ChargeAttack, changeAttackState);

            walkState.AddTransition(StateEnum.Idle, idleState);
            walkState.AddTransition(StateEnum.Dash, dashState);
            walkState.AddTransition(StateEnum.Attack, baseAttackState);
            walkState.AddTransition(StateEnum.ChargeAttack, changeAttackState);

            baseAttackState.AddTransition(StateEnum.Idle, idleState);

            changeAttackState.AddTransition(StateEnum.Idle, idleState);

            dashState.AddTransition(StateEnum.Idle, idleState);

            stateList.Add(idleState);
            stateList.Add(walkState);
            stateList.Add(dashState);
            stateList.Add(baseAttackState);
            stateList.Add(changeAttackState);

            InitializeComponents(stateList);

            Fsm.SetInit(idleState, StateEnum.Idle);
        }

        private void FixedUpdate()
        {
            Fsm.OnFixedExecute();
        }

        public void ChangeToIdle()
        {
            Fsm.Transition(StateEnum.Idle);
        }

        public void ChangeToMove()
        {
            Fsm.Transition(StateEnum.Walk);
        }

        public void ChangeToAttack()
        {
            Fsm.Transition(StateEnum.Attack);
        }

        public void ChangeToChargeAttack()
        {
            Fsm.Transition(StateEnum.ChargeAttack);
        }

        public void ChangeToDash()
        {
            Fsm.Transition(StateEnum.Dash);
        }
    }
}