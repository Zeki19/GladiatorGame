using UnityEngine;

namespace Enemies.FirstBossTest.States
{
    internal class FirstBossStateRunAway<T> : State_Steering<T>
    {
        public FirstBossStateRunAway(ISteering steering, StObstacleAvoidance avoidStObstacles, Transform self) : base(steering, avoidStObstacles, self)
        {

        }
    
        public override void Enter()
        {
            base.Enter();
            Debug.Log("Runaway");
            _move.ModifySpeed(3f);
            _look.PlayStateAnimation(StateEnum.Runaway);
        }
    }
}