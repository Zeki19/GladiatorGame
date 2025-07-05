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
        void Dead()
        {
            transform.parent.gameObject.SetActive(false);
            ServiceLocator.Instance.GetService<SceneChanger>().ChangeScene(3);
        }

        private void Start()
        {
            manager.HealthComponent.OnDead += Dead;
            var playerManager = manager as PlayerManager;
            _phaseSystem = new PhaseSystem(playerManager.stats.stateThreshold, manager.HealthComponent);
            InitializeFsm();
        }

        protected override void InitializeFsm()
        {
            Fsm = new FSM<StateEnum>();
            var move = GetComponent<IMove>();
            var look = GetComponent<ILook>();
            var attack = GetComponent<IAttack>();

            var stateList = new List<PSBase<StateEnum>>();

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

            foreach (var state in stateList)
            {
                state.Initialize(move, look, attack);
            }

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