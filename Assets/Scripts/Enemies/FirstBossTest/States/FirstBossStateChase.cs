using UnityEngine;

namespace Enemies.FirstBossTest.States
{
    internal class FirstBossStateChase<T> : State_Steering<T>
    {
        private Transform _target;
        public Vector2 lastSeenPositionOfTarget;
        private float _attackRange;
        LeaderBehaviour _leaderBehaviour;
        LayerMask _boidMask;
        SpriteRenderer _spriteRenderer;
        public FirstBossStateChase(ISteering steering, StObstacleAvoidance avoidStObstacles, Transform self, Transform target,float attackRange, LeaderBehaviour leaderBehaviour, LayerMask boidMask, SpriteRenderer spriteRenderer) : base(steering, avoidStObstacles, self)
        {
            _target = target;
            _attackRange = attackRange;
            _leaderBehaviour = leaderBehaviour;
            _boidMask = boidMask;
            _spriteRenderer = spriteRenderer;   
        }
        public override void Enter()
        {
            base.Enter();
            Debug.Log("Chase");
            _move.ModifySpeed(1.2f);
            _look.PlayStateAnimation(StateEnum.Chase);
            Transform test = _leaderBehaviour.GetLeader(_move as IBoid, 5, _boidMask);
            _leaderBehaviour.Leader = test;
            _leaderBehaviour.IsActive = true;
            _spriteRenderer.color = Color.yellow;
        }

        public override void Execute()
        {
            base.Execute();

            Vector2 dir = _steering.GetDir(); 
            _move.Move(dir);

            if (dir != Vector2.zero)
                _look.LookDir(dir);

            lastSeenPositionOfTarget = _target.position;

            if (Vector2.Distance(_self.position, _target.position) < _attackRange)
                _move.Move(Vector2.zero);
        }

        public override void Exit()
        {
            base.Exit();
            _move.ModifySpeed(-1.2f);
            _leaderBehaviour.IsActive = false;
        }
    }
}