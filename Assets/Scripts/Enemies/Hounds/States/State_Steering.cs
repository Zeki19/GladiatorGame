using UnityEngine;
    public class State_Steering<T> : States_Base<T>
    {
        protected ISteering _steering;
        protected ObstacleAvoidance _avoidObstacles;
        protected Transform _self;

        public State_Steering(ISteering steering,ObstacleAvoidance avoidObstacles, Transform self)
        {
            _steering = steering;
            _self = self;
            _avoidObstacles = avoidObstacles;

        }
        public override void Execute()
        {
            base.Execute();
            var dir = _avoidObstacles.GetDir(_self, _steering.GetDir());
            _move.Move(dir);
            _look.LookDir(dir);
        } 
        public override void Exit()
        {
            base.Exit();
            _move.ModifySpeed(1f);
        }
    }