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
            _leaderBehaviour.Leader = _target;
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

            if (Vector2.Distance(_self.position, _target.position) < _attackRange)
                _move.Move(Vector2.zero);
        }

        public override void Exit()
        {
            base.Exit();
            _move.ModifySpeed(-1.2f);
            lastSeenPositionOfTarget = _target.position;
            var _model = _move as FirstBossModel;
            _model.lastSeenPlayerPosition = lastSeenPositionOfTarget;
            _leaderBehaviour.IsActive = false;
        }
    }
}