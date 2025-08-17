using Entities;
using Entities.StateMachine;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies.Gaius.States
{
    public class ValeriaStateChase<T> : StatesBase<T>
    {
        private ISteering _steering;
        private Rigidbody2D _target;
        private float _desiredDistance;
        private float _stoppingThreshold;

        public ValeriaStateChase(ISteering steering, Rigidbody2D target, float desiredDistance, float stoppingThreshold)
        {
            _steering = steering;
            _target = target;
            _desiredDistance = desiredDistance;
            _stoppingThreshold = stoppingThreshold;
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
            MaintainDistance();
            Vector2 dir = _steering.GetDir();
            _look.LookDir(dir);
        }
        private void MaintainDistance()
        {
            Vector2 toPlayer = _target.position - _move.Position;
            float dist = toPlayer.magnitude;

            if (dist > _desiredDistance + _stoppingThreshold)
            {
                // Too far → move closer
                MoveToRing(toPlayer);
            }
            else if (dist < _desiredDistance - _stoppingThreshold)
            {
                // Too close → move back
                MoveToRing(toPlayer);
            }
            else
            {
                // Good distance → stop
                _agent._NVagent.ResetPath();
            }
        }

        private void MoveToRing(Vector2 toPlayer)
        {
            Vector2 target = _target.position - toPlayer.normalized * _desiredDistance;
            _agent._NVagent.SetDestination(target);
        }


        public override void Exit()
        {
            base.Exit();
        }
    }
}
