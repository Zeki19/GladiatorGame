using Entities.StateMachine;
using UnityEngine;

namespace Enemies.Valeria.States
{
    public class ValeriaStateAmbush<T> : StatesBase<T>
    {
        private Rigidbody2D _player;
        private float _flankSpeed;
        private float _regularSpeed;

        private float _radius = 1;        
        private float _arcDuration = .5f;    
        private float _elapsedTime;

        private float _startAngle;
        private float _targetAngle;
        private bool _clockwise = true;       

        public bool FinishedFlank { get; private set; }

        public ValeriaStateAmbush(Rigidbody2D player, float flankSpeed)
        {
            _player = player;
            _flankSpeed = flankSpeed;
        }

        public override void Enter()
        {
            base.Enter();

            _move.Move(Vector2.zero);
            _regularSpeed = _agent._NVagent.speed;

            Vector2 offset = _move.Position - _player.position;
            _startAngle = Mathf.Atan2(offset.y, offset.x);

            _targetAngle = _startAngle + (_clockwise ? Mathf.PI : -Mathf.PI);

            _agent._NVagent.speed = _flankSpeed;

            _agent._NVagent.updateRotation = false;
            _agent._NVagent.updateUpAxis = false;

            _elapsedTime = 0f;
            FinishedFlank = false;
        }

        public override void Execute()
        {
            base.Execute();

            _elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(_elapsedTime / _arcDuration);

            float angle = Mathf.Lerp(_startAngle, _targetAngle, t);

            Vector2 playerPos = _player.position;
            Vector2 orbitPos = playerPos + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * _radius;

            _agent._NVagent.SetDestination(orbitPos);

            if (t >= 1f)
            {
                FinishedFlank = true;
            }
        }

        public override void Exit()
        {
            base.Exit();
            _agent._NVagent.speed = _regularSpeed;
            _agent._NVagent.ResetPath(); // clear any current path
            _agent._NVagent.velocity = Vector3.zero; // kill current movement
        }

        
    }
}
