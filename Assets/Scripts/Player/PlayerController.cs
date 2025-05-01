using System.Collections.Generic;
using Entities;
using Interfaces;
using Player.PlayerStates;
using HealthSystem;
using UnityEngine;

namespace Player
{
    public class PlayerController : EntityController
    {
        [SerializeField] private float dashForce;
        [SerializeField] private float dashDuration;
        [SerializeField] private float dashCooldown;
        [SerializeField] private float dashInvincibility;
        
        private IHealth _playerHealth;

        void Dead()
        {
            transform.parent.gameObject.SetActive(false);
            ServiceLocator.Instance.GetService<SceneChanger>().ChangeScene(3);
        }
        private void Awake()
        {
            InitializeFsm();

            _playerHealth = new HealthSystem.HealthSystem(100);

            _playerHealth.OnDead += Dead;
        }

        protected override void InitializeFsm()
        {
            Fsm = new FSM<StateEnum>();
            var move = GetComponent<IMove>();
            var look = GetComponent<ILook>();
            var attack = GetComponent<IAttack>();

            var stateList = new List<PSBase<StateEnum>>();

            var idleState = new PSIdle<StateEnum>();
            var walkState = new PSWalk<StateEnum>();
            var attackState = new PSAttack<StateEnum>(StateEnum.Idle);
            var dashState = new PSDash<StateEnum>(StateEnum.Idle, dashForce, dashDuration, dashCooldown, dashInvincibility, _playerHealth, this);

            idleState.AddTransition(StateEnum.Walk, walkState);
            idleState.AddTransition(StateEnum.Attack, attackState);
            idleState.AddTransition(StateEnum.Dash, dashState);

            walkState.AddTransition(StateEnum.Idle, idleState);
            walkState.AddTransition(StateEnum.Attack, attackState);
            walkState.AddTransition(StateEnum.Dash, dashState);


            attackState.AddTransition(StateEnum.Idle, idleState);

            dashState.AddTransition(StateEnum.Idle, idleState);

            stateList.Add(idleState);
            stateList.Add(walkState);
            stateList.Add(attackState);
            stateList.Add(dashState);

            foreach (var state in stateList)
            {
                state.Initialize(move, look, attack);
            }

            Fsm.SetInit(idleState);
        }
        private void FixedUpdate()
        {
            Fsm.OnFixedExecute();
        }

        public void ChangeToMove()
        {
            Fsm.Transition(StateEnum.Walk);
        }
        public void ChangeToAttack()
        {
            Fsm.Transition(StateEnum.Attack);
        }
        public void ChangeToDash()
        {
            Fsm.Transition(StateEnum.Dash);
        }

    }
}
