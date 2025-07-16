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
        public FirstBossStateChase(ISteering steering, StObstacleAvoidance avoidStObstacles, Transform self, Transform target,float attackRange, LeaderBehaviour leaderBehaviour, LayerMask boidMask, SpriteRenderer spriteRenderer) : base(steering, avoidStObstacles, self)
        {
            _target = target;
            _attackRange = attackRange;
            _leaderBehaviour = leaderBehaviour;
            _boidMask = boidMask;
        }
        public override void Enter()
        {
            base.Enter();
            _move.ModifySpeed(1.2f);
            _animate.PlayStateAnimation(StateEnum.Chase);
            _leaderBehaviour.Leader = _target;
            _leaderBehaviour.IsActive = true;
        }

        public override void Execute()
        {
            base.Execute();

            Vector2 dir = _steering.GetDir(); 
            _move.Move(dir);

            if (dir != Vector2.zero)
                _look.LookDir(dir);

            if (Vector2.Distance(_self.position, _target.position) < _attackRange)
                _move.Move(Vector2.zero);
        }

        public override void Exit()
        {
            base.Exit();
            _move.ModifySpeed(-1.2f);
            lastSeenPositionOfTarget = _target.position;
            var model = _move as FirstBossModel;
            model.lastSeenPlayerPosition = lastSeenPositionOfTarget;
            _leaderBehaviour.IsActive = false;
        }
    }
}