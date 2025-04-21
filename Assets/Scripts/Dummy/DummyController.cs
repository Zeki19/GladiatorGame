using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class DummyController : MonoBehaviour
    {
        public Transform target;
        private FSM<StateEnum> _fsm;
        private DummyModel _model;
        private LineOfSight _los;
        private ITreeNode _root;

        private void Awake()
        {
            _model = GetComponent<DummyModel>();
            _los = GetComponent<LineOfSight>();
        }

        void Start()
        {
            InitializedFsm();
            InitializedTree();
        }

        void Update()
        {
            _fsm.OnExecute();
            _root.Execute();
        }

        private void FixedUpdate()
        {
            _fsm.OnFixedExecute();
        }

        void InitializedFsm()
        {
            
            _fsm = new FSM<StateEnum>();

            var move = GetComponent<IMove>();
            var look = GetComponent<ILook>();
            var attack = GetComponent<IAttack>();
        
            var idleState = new DSIdle<StateEnum>();
            var attackState = new DSAttack<StateEnum>();
            var chaseState = new DSChase<StateEnum>(target, 5f);

            var stateList = new List<DSBase<StateEnum>>
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
            return Vector2.Distance(_model.Position, target.position) <= _model.attackRange;
        }
    
        bool QuestionTargetInView()
        {
            return target != null && _los.LOS(target.transform);
        }
    }
}
