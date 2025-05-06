using System.Collections.Generic;
using Dummy.DummyStates;
using Interfaces;
using UnityEngine;

namespace Dummy
{
    public class DummyController : EnemyController
    {
        #region Private variables
        
        private LineOfSight _los;
        
        //Ref to each State
        private DSChase<StateEnum> _chaseState;
        private DSIdle<StateEnum> _idleState;
        private DSAttack<StateEnum> _attackState;
        //Ref to Steering
        private ISteering _pursuitSteering;
        private float AttackRange;
        
        #endregion
        protected override void Start()
        {
            base.Start();
        }
        protected override void InitializeFsm()
        {
            
            Fsm = new FSM<StateEnum>();

            var move = GetComponent<IMove>();
            var look = GetComponent<ILook>();
            var attack = GetComponent<IAttack>();
        
            var idleState = new DSIdle<StateEnum>();

            var stateList = new List<States_Base<StateEnum>>
            {
                idleState,
            };

            foreach (var t in stateList)
            {
                t.Initialize(move, look, attack);
            }
        
            Fsm.SetInit(idleState);
        }
        protected override void InitializeTree()
        {
            var aIdle = new ActionNode(()=> Fsm.Transition(StateEnum.Idle));

            Root = aIdle;
        }
    }
}
