using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace Dummy
{
    public class DummyController : MonoBehaviour
    {
        [Header("Required scripts")]
        [SerializeField] private Rigidbody2D target; //This is the target it will attack

        #region Private variables
        
        private DummyModel _model;
        private DummyView _view;
        
        private LineOfSight _los;
        
        private ITreeNode _root;
        
        //Ref to each State
        private FSM<StateEnum> _fsm;
        private DSChase<StateEnum> _chaseState;
        private DSIdle<StateEnum> _idleState;
        private DSAttack<StateEnum> _attackState;
        //Ref to Steering
        private ISteering _steering;
        private ISteering _pursuitSteering;
        
        #endregion
        
        private StateEnum _currentState;

        private void Awake()
        {
            _model = GetComponent<DummyModel>();
            _view = GetComponent<DummyView>();
            _los = GetComponent<LineOfSight>();
        }

        void Start()
        {
            _pursuitSteering = new Pursuit(_model.transform, target);
            InitializedFsm();
            InitializedTree();
        }

        void Update()
        {
            _fsm.OnExecute();
            _root.Execute();
        }
        
        void InitializedFsm()
        {
            
            _fsm = new FSM<StateEnum>();

            var move = GetComponent<IMove>();
            var look = GetComponent<ILook>();
            var attack = GetComponent<IAttack>();
        
            var idleState = new DSIdle<StateEnum>();
            var attackState = new DSAttack<StateEnum>();
            var chaseState = new DSChase<StateEnum>(_pursuitSteering, _model.transform);

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
        
            _fsm.SetInit(idleState);
        }

        void InitializedTree()
        {
            var aIdle = new ActionNode(()=>_fsm.Transition(StateEnum.Idle));
            var aChase = new ActionNode(() => _fsm.Transition(StateEnum.Chase));
            var aAttack = new ActionNode(() => _fsm.Transition(StateEnum.Attack));

            var qCanAttack = new QuestionNode(QuestionCanAttack, aAttack, aChase);
            var qTargetInView = new QuestionNode(QuestionTargetInView, qCanAttack, aIdle);

            _root = qTargetInView;
        }

        bool QuestionCanAttack()
        {
            return _los.CheckRange(target.transform,_model.AttackRange);
        }
    
        bool QuestionTargetInView()
        {
            return target != null && _los.LOS(target.transform);
        }
    }
}
