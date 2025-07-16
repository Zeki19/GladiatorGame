using System.Collections.Generic;
using Dummy.DummyStates;
using Enemies;
using Entities.Interfaces;
using Entities.StateMachine;
using UnityEngine;

namespace Dummy
{
    public class DummyController : EnemyController
    {
        #region Private variables
        
        
        //Ref to each State
        private DSChase<StateEnum> _chaseState;
        private DSIdle<StateEnum> _idleState;
        private DSAttack<StateEnum> _attackState;
        //Ref to Steering
        private ISteering _pursuitSteering;
        private float AttackRange;
        
        #endregion

        protected override void InitializeFsm()
        {
            
            Fsm = new FSM<StateEnum>();

            var move = GetComponent<IMove>();
            var look = GetComponent<ILook>();
            var attack = GetComponent<IAttack>();
        
            var idleState = new DSIdle<StateEnum>();

            var stateList = new List<StatesBase<StateEnum>>
            {
                idleState,
            };

            foreach (var t in stateList)
            {
                t.Initialize(move, look, attack);
            }
        
            Fsm.SetInit(idleState,StateEnum.Idle);
        }
        protected override void InitializeTree()
        {
            var aIdle = new ActionNode(()=> Fsm.Transition(StateEnum.Idle));

            //Root = aIdle;
        }
    }
}
