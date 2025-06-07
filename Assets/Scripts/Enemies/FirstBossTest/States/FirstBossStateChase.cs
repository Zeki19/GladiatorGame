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
        public FirstBossStateChase(ISteering steering, StObstacleAvoidance avoidStObstacles, Transform self, Transform target,float attackRange, LeaderBehaviour leaderBehaviour, LayerMask boidMask) : base(steering, avoidStObstacles, self)
        {
            _target = target;
            _attackRange = attackRange;
            _leaderBehaviour = leaderBehaviour;
            _boidMask = boidMask;
        }
        public override void Enter()
        {
            base.Enter();
            Debug.Log("Chase");
            _move.ModifySpeed(1.2f);
            _look.PlayStateAnimation(StateEnum.Chase);
            _leaderBehaviour.GetLeader(_move as IBoid, 10, _boidMask);
            _leaderBehaviour.IsActive = true;
        }

        public override void Execute()
        {
            base.Execute();
            if(Vector2.Distance(_self.position,_target.position)<_attackRange)
                _move.Move(Vector2.zero);
            lastSeenPositionOfTarget = _target.position;
        }

        public override void Exit()
        {
            base.Exit();
            _move.ModifySpeed(-1.2f);
            _leaderBehaviour.IsActive = false;
        }
    }
}