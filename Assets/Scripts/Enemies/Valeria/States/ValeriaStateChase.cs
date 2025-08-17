using Entities.StateMachine;
using UnityEngine;

namespace Enemies.Gaius.States
{
    public class ValeriaStateChase<T> : StatesBase<T>
    {
        private ISteering _steering;
        private Rigidbody2D _target;
        private float _desiredDistance;
        private float _stoppingThreshold;
        private float _orbitSpeed;
        private float _orbitAngle;

        public ValeriaStateChase(ISteering steering, Rigidbody2D target, float desiredDistance, float stoppingThreshold, float orbitSpeed, float orbitAngle)
        {
            _steering = steering;
            _target = target;
            _desiredDistance = desiredDistance;
            _stoppingThreshold = stoppingThreshold;
            _orbitSpeed = orbitSpeed;
            _orbitAngle = orbitAngle;
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

            if (dist > _desiredDistance + _stoppingThreshold || dist < _desiredDistance - _stoppingThreshold)
            {
                MoveToRing(toPlayer);
            }
            else
            {
                OrbitAroundPlayer(toPlayer);
            }
        }

        private void MoveToRing(Vector2 toPlayer)
        {
            Vector2 target = _target.position - toPlayer.normalized * _desiredDistance;
            _agent._NVagent.SetDestination(target);
        }
        private void OrbitAroundPlayer(Vector2 toPlayer)
        {
            _orbitAngle += _orbitSpeed * Time.deltaTime;
            Vector2 orbitOffset = new Vector2(Mathf.Cos(_orbitAngle), Mathf.Sin(_orbitAngle)) * _desiredDistance;
            Vector2 orbitTarget = (Vector2)_target.position + orbitOffset;
            _agent._NVagent.SetDestination(orbitTarget);
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
