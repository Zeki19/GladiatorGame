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
        private Rigidbody2D target;
        private float _longAttackCd=3;
        private float _longTimerCd;
        public GaiusStateChase(ISteering steering, GaiusController controller, Rigidbody2D target)
        {
            _steering = steering;
            _speedMod = controller.stats.Stack;
            _speedModeInterval = controller.stats.Interval;
            this.target = target;
        }

        public override void Enter()
        {
            base.Enter();
            _move.Move(Vector2.zero);
            _animate.PlayStateAnimation(StateEnum.Chase);
            _timer = _speedModeInterval;
            _agent._NVagent.updateRotation = false;
            _agent._NVagent.updateUpAxis = false;
        }
        public override void Execute()
        {
            base.Execute();
            _agent._NVagent.SetDestination(new Vector3 (target.position.x, target.position.y, 0));
            Vector2 dir = _steering.GetDir();
            _longTimerCd += Time.deltaTime;
            if (_longTimerCd > _longAttackCd)
            {
                _status.SetStatus(StatusEnum.OnLongCD,true);
                _longTimerCd = 0;
            }
            _timer -= Time.deltaTime;

            if(_timer<0)
            {
                _agent._NVagent.speed += _speedMod;
                _stackingSpeed += _speedMod;
                _timer = _speedModeInterval;
            }

            _move.Move(dir);
            _look.LookDir(dir);
        }
        public override void Exit()
        {
            base.Exit();
            _agent._NVagent.speed -= _speedMod;
            _stackingSpeed = 0;
        }
    }
}
