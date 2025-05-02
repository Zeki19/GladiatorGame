using UnityEngine;
    public class State_Steering<T> : States_Base<T>
    {
        protected ISteering _steering;
        protected StObstacleAvoidance AvoidStObstacles;
        protected Transform _self;

        public State_Steering(ISteering steering,StObstacleAvoidance avoidStObstacles, Transform self)
        {
            _steering = steering;
            _self = self;
            AvoidStObstacles = avoidStObstacles;

        }
        public override void Execute()
        {
            base.Execute();
            var dir = AvoidStObstacles.GetDir(_self, _steering.GetDir());
            _move.Move(dir);
            _look.LookDir(dir);
        } 
        public override void Exit()
        {
            base.Exit();
            _move.ModifySpeed(1f);
        }
    }