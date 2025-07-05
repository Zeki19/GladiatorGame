using System.Collections.Generic;
using Enemies.Gaius.States;
using Entities.Interfaces;
using Entities.StateMachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Enemies.Gaius
{
    public class GaiusController : EnemyController
    {
        [SerializeField] public GaiusStatsSO stats;

        public SpriteRenderer SpriteRendererBoss;

        public GameObject weapom; //WEAPOMMMMMMMMM
        [SerializeField]private List<AnimationCurve> curves;

        public bool didAttackMiss = false;
        public bool isAttacking = false;
        public bool isBackStepFinished;
        public bool FinishedAttacking;
        public bool canLongAttack = true;

        #region Private Variables

        private States_Base<StateEnum> _idleState; // BLUE
        private States_Base<StateEnum> _backStepState; // BLUE
        private States_Base<StateEnum> _chaseState; // WHITE
        public States_Base<StateEnum> _shortAttackState; // YELLOW
        private States_Base<StateEnum> _midAttackState; // RED
        private States_Base<StateEnum> _longAttackState; // BLACK

        private ISteering _pursuitSteering;
        private StObstacleAvoidance _avoidWalls;

        #endregion

        protected override void Awake()
        {
            InitalizeSteering();
            // The ranges must go before the base.Awake() so that it uploads them to the context of the decision tree.
            attackRanges.Add(0);
            attackRanges.Add(stats.mediumRange);
            attackRanges.Add(stats.longRange);
            base.Awake();
            SpriteRendererBoss = GetComponent<SpriteRenderer>();
        }

        protected override void Start()
        {
            base.Start();
            manager.HealthComponent.OnDamage += CheckPhase;
            manager.HealthComponent.OnDead += Die;
        }

        void InitalizeSteering()
        {
            _pursuitSteering = new StPursuit(transform, target, 0);
            _avoidWalls = new StObstacleAvoidance(stats.maxObs, stats.radius, stats.angleOfVision, stats.personalArea, stats.obsMask);
        }

        protected override void InitializeFsm()
        {
            Fsm = new FSM<StateEnum>();

            var move = GetComponent<IMove>();
            var look = GetComponent<ILook>();
            var attack = GetComponent<IAttack>();

            var idleState = new GaiusStateIdle<StateEnum>( SpriteRendererBoss, this,manager);
            var backStepState = new GaiusStateBackStep<StateEnum>( SpriteRendererBoss, this,manager);
            var chaseState = new GaiusStateChase<StateEnum>(_pursuitSteering,_avoidWalls,transform,SpriteRendererBoss,manager);
            var shortAttackState = new GaiusStateShortAttack<StateEnum>(_pursuitSteering, _avoidWalls, transform, SpriteRendererBoss, this,weapom,curves,manager.view as GaiusView);
            var midAttackState = new GaiusStateMidAttack<StateEnum>(_pursuitSteering, _avoidWalls, transform, SpriteRendererBoss, this);
            var longAttackState = new GaiusStateLongAttack<StateEnum>(_pursuitSteering, _avoidWalls, transform, SpriteRendererBoss, this, weapom, curves, manager);

            _idleState = idleState;
            _backStepState = backStepState;
            _chaseState = chaseState;
            _shortAttackState = shortAttackState;
            _midAttackState = midAttackState;
            _longAttackState = longAttackState;


            var stateList = new List<States_Base<StateEnum>>
            {
                idleState,
                backStepState,
                chaseState,
                shortAttackState,
                midAttackState,
                longAttackState
            };

            idleState.AddTransition(StateEnum.Chase, chaseState);
            idleState.AddTransition(StateEnum.ShortAttack, shortAttackState);
            idleState.AddTransition(StateEnum.MidAttack, midAttackState);
            idleState.AddTransition(StateEnum.LongAttack, longAttackState);
            idleState.AddTransition(StateEnum.BackStep, backStepState);

            chaseState.AddTransition(StateEnum.Idle, idleState);
            chaseState.AddTransition(StateEnum.ShortAttack, shortAttackState);
            chaseState.AddTransition(StateEnum.MidAttack, midAttackState);
            chaseState.AddTransition(StateEnum.LongAttack, longAttackState);
            chaseState.AddTransition(StateEnum.BackStep, backStepState);

            shortAttackState.AddTransition(StateEnum.Idle, idleState);
            shortAttackState.AddTransition(StateEnum.Chase, chaseState);
            shortAttackState.AddTransition(StateEnum.MidAttack, midAttackState);
            shortAttackState.AddTransition(StateEnum.LongAttack, longAttackState);
            shortAttackState.AddTransition(StateEnum.BackStep, backStepState);

            midAttackState.AddTransition(StateEnum.Idle, idleState);
            midAttackState.AddTransition(StateEnum.Chase, chaseState);
            midAttackState.AddTransition(StateEnum.ShortAttack, shortAttackState);
            midAttackState.AddTransition(StateEnum.LongAttack, longAttackState);
            midAttackState.AddTransition(StateEnum.BackStep, backStepState);


            longAttackState.AddTransition(StateEnum.Idle, idleState);
            longAttackState.AddTransition(StateEnum.Chase, chaseState);
            longAttackState.AddTransition(StateEnum.ShortAttack, shortAttackState);
            longAttackState.AddTransition(StateEnum.MidAttack, midAttackState);
            longAttackState.AddTransition(StateEnum.BackStep, backStepState);
            
            backStepState.AddTransition(StateEnum.Idle, idleState);
            backStepState.AddTransition(StateEnum.Chase, chaseState);
            backStepState.AddTransition(StateEnum.ShortAttack, shortAttackState);
            backStepState.AddTransition(StateEnum.MidAttack, midAttackState);
            backStepState.AddTransition(StateEnum.LongAttack, longAttackState);



            foreach (var t in stateList)
            {
                t.Initialize(move, look, attack);
            }

            Fsm.SetInit(chaseState,StateEnum.Chase);
        }

        protected override void InitializeTree()
        {
            Root.Execute(objectContext);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, stats.mediumRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, stats.longRange);
            /*
            if (Fsm.CurrentStateEnum() == StateEnum.ShortAttack)
            {
                Vector3 origin = transform.position;
                Vector3 facingDir = transform.up;

                float halfAngle = stats.shortAngle / 2f;

                Quaternion leftRot = Quaternion.Euler(0, 0, -halfAngle);
                Quaternion rightRot = Quaternion.Euler(0, 0, halfAngle);

                Vector3 leftDir = leftRot * facingDir;
                Vector3 rightDir = rightRot * facingDir;

                Gizmos.color = Color.red;
                Gizmos.DrawLine(origin, origin + leftDir * stats.shortRange);
                Gizmos.DrawLine(origin, origin + rightDir * stats.shortRange);
                Gizmos.DrawWireSphere(origin, stats.shortRange);
            }*/

        }

        private void Die()
        {
            Destroy(gameObject);
            SceneChanger.Instance.ChangeScene(2);
        }

    }
}