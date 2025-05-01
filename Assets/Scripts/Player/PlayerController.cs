using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float dashForce;
        [SerializeField] private float dashDuration;
        [SerializeField] private float dashCooldown;
        [SerializeField] private float dashInvincibility;

        private FSM<StateEnum> _fsm;
        private IHealth _playerHealth;

        void Dead()
        {
            transform.parent.gameObject.SetActive(false);
            ServiceLocator.Instance.GetService<SceneChanger>().ChangeScene(3);
        }
        private void Awake()
        {
            InitializeFsm();
        
            _playerHealth = transform.parent.GetComponent<HealthSystem>();

            _playerHealth.OnDead += Dead;
        }

        private void InitializeFsm()
        {
            _fsm = new FSM<StateEnum>();
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

            _fsm.SetInit(idleState);
        }
        private void Update()
        {
            _fsm.OnExecute();
        }
        private void FixedUpdate()
        {
            _fsm.OnFixedExecute();
        }

        public void ChangeToMove()
        {
            _fsm.Transition(StateEnum.Walk);
        }
        public void ChangeToAttack()
        {
            _fsm.Transition(StateEnum.Attack);
        }
        public void ChangeToDash()
        {
            _fsm.Transition(StateEnum.Dash);
        }

    }
}
