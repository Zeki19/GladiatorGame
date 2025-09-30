using Entities;
using Entities.StateMachine;
using UnityEngine;

namespace Enemies.Minotaur.States
{
    public class MinotaurStateChase<T> : StatesBase<T>
    {
        private ISteering _steering;
        private float _speedMod;
        private float _stackingSpeed;
        private float _speedModeInterval = 4;
        private float _timer;
        private float _longTimerCd;
        public MinotaurStateChase(ISteering steering, MinotaurController controller)
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
            _agent._NVagent.updateRotation = false;
            _agent._NVagent.updateUpAxis = false;
        }
        public override void Execute()
        {
            base.Execute();
            _agent._NVagent.SetDestination(_target.GetTarget().transform.position);
            _longTimerCd += Time.deltaTime;

            _timer -= Time.deltaTime;

            if(_timer<0)
            {
                _agent._NVagent.speed += _speedMod;
                _stackingSpeed += _speedMod;
                _timer = _speedModeInterval;
            }

            Vector2 dir = _steering.GetDir();
            _move.Move(dir);
            _look.LookDir(dir);
        }
        public override void Exit()
        {
            base.Exit();
            _agent._NVagent.speed -= _stackingSpeed;
            _stackingSpeed = 0;
            _agent._NVagent.ResetPath(); // clear any current path
            _agent._NVagent.velocity = Vector3.zero; // kill current movement
        }
    }
}
