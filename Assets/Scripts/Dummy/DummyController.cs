using System.Collections.Generic;
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
        
        #endregion

        private void Awake()
        {
            _los = GetComponent<LineOfSight>();
        }
        protected override void Start()
        {
            base.Start();
            
            _pursuitSteering = new Pursuit(manager.model.transform, target);
        }
        protected override void InitializeFsm()
        {
            
            Fsm = new FSM<StateEnum>();

            var move = GetComponent<IMove>();
            var look = GetComponent<ILook>();
            var attack = GetComponent<IAttack>();
        
            var idleState = new DSIdle<StateEnum>();
            var attackState = new DSAttack<StateEnum>();
            var chaseState = new DSChase<StateEnum>(_pursuitSteering, manager.model.transform);

            var stateList = new List<States_Base<StateEnum>>
            {
                idleState,
                attackState,
                chaseState
            };

            idleState.AddTransition(StateEnum.Chase, chaseState);
            idleState.AddTransition(StateEnum.Attack, attackState);
        
            attackState.AddTransition(StateEnum.Chase, chaseState);
            attackState.AddTransition(StateEnum.Idle, idleState);
        
            chaseState.AddTransition(StateEnum.Attack, attackState);
            chaseState.AddTransition(StateEnum.Idle, idleState);

            foreach (var t in stateList)
            {
                t.Initialize(move, look, attack);
            }
        
            Fsm.SetInit(idleState);
        }
        protected override void InitializeTree()
        {
            var aIdle = new ActionNode(()=> Fsm.Transition(StateEnum.Idle));
            var aChase = new ActionNode(() => Fsm.Transition(StateEnum.Chase));
            var aAttack = new ActionNode(() => Fsm.Transition(StateEnum.Attack));

            var qCanAttack = new QuestionNode(QuestionCanAttack, aAttack, aChase);
            var qTargetInView = new QuestionNode(QuestionTargetInView, qCanAttack, aIdle);

            Root = qTargetInView;
        }
        bool QuestionCanAttack()
        {
            return _los.CheckRange(target.transform, manager.model.AttackRange);
        }
        bool QuestionTargetInView()
        {
            return target != null && _los.LOS(target.transform);
        }
    }
}
