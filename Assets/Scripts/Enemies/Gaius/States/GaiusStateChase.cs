using Entities;
using Entities.StateMachine;
using UnityEngine;

namespace Enemies.Gaius.States
{
    public class GaiusStateChase<T> : StatesBase<T>
    {
        private ISteering _steering;
        private float _speedMod;
        private float _stackingSpeed;
        private float _speedModeInterval = 1;
        private float _timer;
        public GaiusStateChase(ISteering steering, GaiusController controller)
        {
            _steering = steering;
            _speedMod = controller.stats.Stack;
            _speedModeInterval = controller.stats.Interval;
        }

        public override void Enter()
        {
            base.Enter();
            _move.Move(Vector2.zero);
            _animate.PlayStateAnimation(StateEnum.Chase);
            _timer = _speedModeInterval;
        }
        public override void Execute()
        {
            base.Execute();

            Vector2 dir = _steering.GetDir();
            _timer -= Time.deltaTime;
            if(_timer<0)
            {
                _move.ModifySpeed(_speedMod);
                _stackingSpeed += _speedMod;
                _timer = _speedModeInterval;
            }
            _move.Move(dir);
            _look.LookDir(dir);
        }
        public override void Exit()
        {
            base.Exit();
            _move.ModifySpeed(-_stackingSpeed);
            _stackingSpeed = 0;
        }
    }
}
