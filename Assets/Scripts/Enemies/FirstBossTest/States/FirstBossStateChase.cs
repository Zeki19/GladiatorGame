using UnityEngine;

namespace Enemies.FirstBossTest.States
{
    internal class FirstBossStateChase<T> : State_Steering<T>
    {
        private Transform _target;
        public Vector2 lastSeenPositionOfTarget;
        public FirstBossStateChase(ISteering steering, StObstacleAvoidance avoidStObstacles, Transform self, Transform target) : base(steering, avoidStObstacles, self)
        {
            _target = target;
        }
        public override void Enter()
        {
            base.Enter();
            Debug.Log("Chase");
        
            _move.ModifySpeed(2f);
            _look.PlayStateAnimation(StateEnum.Chase);
        }

        public override void Execute()
        {
            base.Execute();
            lastSeenPositionOfTarget = _target.position;
        }

        public override void Exit()
        {
            base.Exit();
            _move.ModifySpeed(-2f);
        }
    }
}