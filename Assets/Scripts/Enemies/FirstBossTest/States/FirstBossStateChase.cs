using UnityEngine;

namespace Enemies.FirstBossTest.States
{
    internal class FirstBossStateChase<T> : State_Steering<T>
    {
        private Transform _target;
        public Vector2 lastSeenPositionOfTarget;
        private float _attackRange;
        public FirstBossStateChase(ISteering steering, StObstacleAvoidance avoidStObstacles, Transform self, Transform target,float attackRange) : base(steering, avoidStObstacles, self)
        {
            _target = target;
            _attackRange = attackRange;
        }
        public override void Enter()
        {
            base.Enter();
            Debug.Log("Chase");
        
            _move.ModifySpeed(1.2f);
            _look.PlayStateAnimation(StateEnum.Chase);
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
        }
    }
}