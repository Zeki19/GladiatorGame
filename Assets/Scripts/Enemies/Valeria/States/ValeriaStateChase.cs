using Entities;
using Entities.StateMachine;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies.Gaius.States
{
    public class ValeriaStateChase<T> : StatesBase<T>
    {
        private ISteering _steering;
        private float _stackingSpeed;
        private float _timer;
        private Rigidbody2D _target;
        public ValeriaStateChase(ISteering steering, Rigidbody2D target)
        {
            _steering = steering;
            _target = target;
        }

        public override void Enter()
        {
            base.Enter();
            _move.Move(Vector2.zero);
            _animate.PlayStateAnimation(StateEnum.Chase);
            _agent._NVagent.updateRotation = false;
            _agent._NVagent.updateUpAxis = false;
        }
        public override void Execute()
        {
            base.Execute();
            _agent._NVagent.SetDestination(new Vector3 (_target.position.x, _target.position.y, 0));
            Vector2 dir = _steering.GetDir();
            /*
            _timer -= Time.deltaTime;
            if(_timer<0)
            {
                _move.ModifySpeed(_speedMod);
                _stackingSpeed += _speedMod;
                _timer = _speedModeInterval;
            }

            _move.Move(dir);
            */
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
